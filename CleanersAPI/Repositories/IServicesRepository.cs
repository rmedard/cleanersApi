using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IServicesRepository : ICleanersRepository<Service>
    {
        Task<IEnumerable<Service>> GetServicesByCategory(Category category);
    }
}