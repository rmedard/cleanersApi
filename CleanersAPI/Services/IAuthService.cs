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
        User GenerateUserAccount(Professional professional, string password);
        User GenerateUserAccount(Customer customer, string password);
        Task<User> GetUserById(int userId);
    }
}