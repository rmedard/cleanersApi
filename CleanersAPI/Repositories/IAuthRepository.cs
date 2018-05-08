using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);
    }
}