using System;
using System.Collections.Generic;
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
            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(u => username.Equals(u.Email));
            if (user == null)
            {
                return null;
            }
            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? user : null;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.Email == username);
        }

        public Role GetRoleByName(RoleName roleName)
        {
            return _context.Roles.First(r => r.RoleName == roleName);
        }

        public Task<User> GetById(int userId)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));
        }

        private static bool VerifyPasswordHash(string password, IReadOnlyList<byte> passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt); //Because HMACSHA512() implements IDisposable
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != passwordHash[i]).Any();
        }
    }
}