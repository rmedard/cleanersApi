using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public Task<User> Login(string username, string password)
        {
            return _authRepository.Login(username, password);
        }

        public Task<bool> UserExists(string username)
        {
            return _authRepository.UserExists(username);
        }
    }
}