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
            return await _context.Expertises.ToListAsync();
        }

        public async Task<Expertise> GetOne(int professionalId, int professionId)
        {
            return await _context.Expertises.FirstOrDefaultAsync(exp =>
                exp.ProfessionalId == professionalId && exp.ServiceId == professionId);
        }

        public bool DoesExist(int professionalId, int professionId)
        {
            return _context.Expertises.Any(exp =>
                exp.ProfessionalId == professionalId && exp.ServiceId == professionId);
        }
    }
}