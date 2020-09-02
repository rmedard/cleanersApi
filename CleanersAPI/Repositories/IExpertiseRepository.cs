using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IExpertiseRepository
    {
        Task<IEnumerable<Expertise>> GetAll();
        Task<Expertise> GetOne(int professionalId, int serviceId);
        bool DoesExist(int professionalId, int professionId);
    }
}