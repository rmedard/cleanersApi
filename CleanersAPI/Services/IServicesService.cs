using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IServicesService : ICleanersService<Service>
    {
        Task<IEnumerable<Service>> GetServicesByCategory(Category category);
    }
}