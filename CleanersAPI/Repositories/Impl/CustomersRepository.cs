using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories.Impl
{
    public class CustomersRepository : ICustomersRepository
    {
        
        private readonly CleanersApiContext _context;

        public CustomersRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Customer>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Customer> GetById(int id)
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

        public Task<Customer> Update(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}