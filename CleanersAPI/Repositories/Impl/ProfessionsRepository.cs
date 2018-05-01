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

        public async Task<IEnumerable<Profession>> GetAll()
        {
            return await _context.Professions.ToListAsync();
        }

        public Task<Profession> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool DoesExist(int professionId)
        {
            return _context.Professions.Any(p => p.Id == professionId);
        }

        public Task<Profession> Create(Profession profession)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Profession profession)
        {
            _context.Entry(profession).State = EntityState.Modified;
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
