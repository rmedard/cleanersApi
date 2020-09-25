using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class ExpertiseRepository : IExpertiseRepository
    {
        private readonly CleanersApiContext _context;

        public ExpertiseRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expertise>> GetAll()
        {
            return await _context.Expertises
                .Include(e => e.Service)
                .Include(e => e.Professional)
                .ToListAsync();
        }

        public async Task<Expertise> GetById(int id)
        {
            return await _context.Expertises
                .Include(e => e.Service)
                .Include(e => e.Professional)
                .FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public bool DoesExist(int id)
        {
            return _context.Expertises.Any(e => e.Id == id);
        }

        public Task<Expertise> Create(Expertise customer)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Update(Expertise expertise)
        {
            _context.Entry(expertise).State = EntityState.Modified;
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
            throw new System.NotImplementedException();
        }

        public async Task<Expertise> GetOne(int professionalId, int serviceId)
        {
            return await _context.Expertises.FirstOrDefaultAsync(exp =>
                exp.ProfessionalId == professionalId && exp.ServiceId == serviceId);
        }

        public async Task<IEnumerable<Expertise>> GetExpertisesByServiceId(int serviceId)
        {
            return await _context.Expertises
                .Include(e => e.Professional)
                .ThenInclude(p => p.Address)
                .Where(e => e.Active && e.ServiceId.Equals(serviceId)).ToListAsync();
        }
    }
}