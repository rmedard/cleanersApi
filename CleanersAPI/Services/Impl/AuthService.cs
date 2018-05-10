using System.Text;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IProfessionalsRepository _professionalsRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IUsersRepository _usersRepository;

        public AuthService(IAuthRepository authRepository, 
            IProfessionalsRepository professionalsRepository,
            ICustomersRepository customersRepository,
            IUsersRepository usersRepository)
        {
            _authRepository = authRepository;
            _professionalsRepository = professionalsRepository;
            _customersRepository = customersRepository;
            _usersRepository = usersRepository;
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
                PasswordSalt = passwordSalt
            };
            var newRoleUser = new RoleUser
            {
                role = new Role {Name = RoleName.USER}, 
                user = newUser
            };
            professional.User = newUser;
            _professionalsRepository.Update(professional);
            _usersRepository.CreateRoleUser(newRoleUser);
        }

        public void AddUserToCustomer(Customer customer, UserForLoginDto userForLoginDto)
        {
            CreatePasswordHash(userForLoginDto.Password, out var passwordHash, out var passwordSalt);
            var newUser = new User {
                Username = userForLoginDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            var newRoleUser = new RoleUser
            {
                role = new Role {Name = RoleName.USER}, 
                user = newUser
            };
            customer.User = newUser;
            _customersRepository.Update(customer);
            _usersRepository.CreateRoleUser(newRoleUser);
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