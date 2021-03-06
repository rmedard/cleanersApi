﻿using System;
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
            var trackedUser = _context.Users
                .Include(u => u.Address)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .First(u => u.Id.Equals(user.Id));

            _context.Entry(trackedUser.Address).CurrentValues.SetValues(user.Address);
            _context.Entry(trackedUser).Property(u => u.Picture).CurrentValue = user.Picture;
            _context.Entry(trackedUser).Property(u => u.FirstName).CurrentValue = user.FirstName;
            _context.Entry(trackedUser).Property(u => u.LastName).CurrentValue = user.LastName;
            _context.Entry(trackedUser).Property(u => u.PhoneNumber).CurrentValue = user.PhoneNumber;
            _context.Entry(trackedUser).Property(u => u.IsActive).CurrentValue = user.IsActive;
            
            _context.Entry(trackedUser.Address).State = EntityState.Modified;
            _context.Entry(trackedUser).Property(u => u.Picture).IsModified = true;
            _context.Entry(trackedUser).Property(u => u.FirstName).IsModified = true;
            _context.Entry(trackedUser).Property(u => u.LastName).IsModified = true;
            _context.Entry(trackedUser).Property(u => u.PhoneNumber).IsModified = true;
            _context.Entry(trackedUser).Property(u => u.IsActive).IsModified = true;

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

        public Role GetRoleByName(RoleName roleName)
        {
            var roles = _context.Roles.ToListAsync().Result;
            return roles.Find(r => r.RoleName.Equals(roleName));
        }
    }
}