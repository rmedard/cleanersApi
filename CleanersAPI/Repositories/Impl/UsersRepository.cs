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
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .ToListAsync();
        }

        public Task<User> GetById(int id)
        {
            return _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public bool DoesExist(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Create(User customer)
        {
            var userEntry = _context.Add(customer).Entity;
            await _context.SaveChangesAsync();
            return userEntry;
        }

        public async Task<bool> Update(User t)
        {
            _context.Entry(t.Address).State = EntityState.Modified;
            _context.Entry(t).Property(u => u.Picture).IsModified = true;
            _context.Entry(t).Property(u => u.FirstName).IsModified = true;
            _context.Entry(t).Property(u => u.LastName).IsModified = true;
            _context.Entry(t).Property(u => u.PhoneNumber).IsModified = true;
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