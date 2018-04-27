using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services.Impl
{
    public class CustomersService : ICustomersService
    {
        public Task<IEnumerable<Customer>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Customer> GetOneById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Customer> Create(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        Task<Customer> ICleanersService<Customer>.Update(Customer t)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Customer customer)
        {
            throw new System.NotImplementedException();
        }
    }
}