using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IExpertiseService : ICleanersService<Expertise>
    {
        Task<Expertise> FindExpertise(int professionalId, int professionId);

        Task<IEnumerable<Expertise>> GetExpertisesByServiceId(int serviceId);
    }
}