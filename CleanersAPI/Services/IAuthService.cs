using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IAuthService
    {
        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);
    }
}