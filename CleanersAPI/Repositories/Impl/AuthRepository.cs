using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class AuthRepository : IAuthRepository
    {

        private readonly CleanersApiContext _context;

        public AuthRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(u => u.Roles).ThenInclude(r => r.Role)
                .Include(u => u.Customer)
                .Include(u => u.Professional)
                .FirstAsync(u => u.Username == username);
            if (user == null)
            {
                return null;
            }
            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? user : null;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username == username);
        }

        public Role GetRoleByName(RoleName roleName)
        {
            return _context.Roles.First(r => r.RoleName == roleName);
        }
        
        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //Because HMACSHA512() implements IDisposable
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return !computedHash.Where((t, i) => t != passwordHash[i]).Any();
            }
        }
    }
}