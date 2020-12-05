using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface ICustomersService : ICleanersService<Customer>
    {
        Task<IEnumerable<Reservation>> GetOrderedServices(int customerId);

        Task<Customer> GetCustomerByUserId(int userId);
        Task<IEnumerable<Customer>> GetAvailableBillableCustomers();
    }
}