using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CleanersApiContext _context;

        public UsersRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public bool DoesExist(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Create(User user)
        {
            var userEntry = _context.Add(user).Entity;
            await _context.SaveChangesAsync();
            return userEntry;
        }

        public async Task<bool> Update(User user)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateRoleUser(RoleUser roleUser)
        {
            _context.RoleUser.Add(roleUser);
            _context.SaveChanges();
        }
    }
}