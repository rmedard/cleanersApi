using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos;
using CleanersAPI.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanersAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IProfessionalsRepository _professionalsRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, 
            IProfessionalsRepository professionalsRepository,
            ICustomersRepository customersRepository, 
            IUsersRepository usersRepository,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _professionalsRepository = professionalsRepository;
            _customersRepository = customersRepository;
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

        public void AddUserToProfessional(Professional professional, UserForLoginDto userForLoginDto)
        {
            CreatePasswordHash(userForLoginDto.Password, out var passwordHash, out var passwordSalt);
            var newUser = new User {
                Username = userForLoginDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Roles = { new RoleUser { role = _authRepository.GetRoleByName(RoleName.User)}}
            };
            
            professional.User = newUser;
            _professionalsRepository.Update(professional);
        }

        public string GenerateLoginToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role,
                        user.Roles.Select(roleUser => roleUser.role).Select(role => role.Name)
                            .Contains(RoleName.Admin) ? RoleName.Admin.ToString() : RoleName.User.ToString())
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public void AddUserToCustomer(Customer customer, UserForLoginDto userForLoginDto)
        {
            CreatePasswordHash(userForLoginDto.Password, out var passwordHash, out var passwordSalt);
            var newUser = new User {
                Username = userForLoginDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Roles = { new RoleUser { role = _authRepository.GetRoleByName(RoleName.User)}}
            };

            customer.User = newUser;
            _customersRepository.Update(customer);
        }

        public Task<User> GetUserById(int userId)
        {
            return _usersRepository.GetById(userId);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) //Because HMACSHA512() implements IDisposable
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}