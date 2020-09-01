using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class ProfessionsRepository : IProfessionsRepository
    {

        private readonly CleanersApiContext _context;

        public ProfessionsRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAll()
        {
            return await _context.Services.ToListAsync();
        }

        public Task<Service> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool DoesExist(int professionId)
        {
            return _context.Services.Any(p => p.Id == professionId);
        }

        public async Task<Service> Create(Service service)
        {
            var newProfession = _context.Services.Add(service).Entity;
            await _context.SaveChangesAsync();
            return newProfession;
        }

        public async Task<bool> Update(Service service)
        {
            _context.Entry(service).State = EntityState.Modified;
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
    }
}
