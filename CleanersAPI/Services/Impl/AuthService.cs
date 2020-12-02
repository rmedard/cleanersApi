using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanersAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository,
            IUsersRepository usersRepository,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        public Task<User> Login(string username, string password)
        {
            return _authRepository.Login(username, password);
        }

        public Task<bool> UserExists(string username)
        {
            return _authRepository.UserExists(username);
        }

        public void GenerateUserAccount(Professional professional, string password)
        {
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            professional.User.PasswordHash = passwordHash;
            professional.User.PasswordSalt = passwordSalt;
            professional.User.Roles.Add(new RoleUser {Role = _authRepository.GetRoleByName(RoleName.Professional)});
        }

        public void GenerateUserAccount(Customer customer, string password)
        {
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            customer.User.PasswordHash = passwordHash;
            customer.User.PasswordSalt = passwordSalt;
            customer.User.Roles.Add(new RoleUser {Role = _authRepository.GetRoleByName(RoleName.Customer)});
        }

        public string GenerateLoginToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Roles.First().Role.RoleName.ToString())
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _usersRepository.GetById(userId);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(); //Because HMACSHA512() implements IDisposable
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}