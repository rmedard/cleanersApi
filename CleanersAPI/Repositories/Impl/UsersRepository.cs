using System;
using System.Collections.Generic;
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
            return await _context.Users.ToListAsync();
        }

        public Task<User> GetById(int id)
        {
            return _context.Users.Include(u => u.Customer)
                .Include(u => u.Professional).SingleOrDefaultAsync(u => u.Id == id);
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
            _context.Entry(t).State = EntityState.Modified;
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