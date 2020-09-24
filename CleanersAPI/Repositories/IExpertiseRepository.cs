using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IExpertiseRepository : ICleanersRepository<Expertise>
    {
        Task<Expertise> GetOne(int professionalId, int serviceId);
        
        Task<IEnumerable<Expertise>> GetExpertisesByServiceId(int serviceId);
    }
}