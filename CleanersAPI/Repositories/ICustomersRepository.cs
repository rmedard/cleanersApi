using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface ICustomersRepository : ICleanersRepository<Customer>
    {
        Task<IEnumerable<Reservation>> GetCustomerOrderedServices(int customerId);

        Task<Customer> GetCustomerByUserId(int userId);
    }
}