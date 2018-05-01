using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface ICustomersService : ICleanersService<Customer>
    {
        Task<IEnumerable<Service>> getOrderedServices(int customerId);
    }
}