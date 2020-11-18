using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.User;

namespace CleanersAPI.Services
{
    public interface IAuthService
    {
        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);
        string GenerateLoginToken(User user);
        void GenerateUserAccount(Professional professional, string password);
        void GenerateUserAccount(Customer customer, string password);
        Task<User> GetUserById(int userId);
    }
}