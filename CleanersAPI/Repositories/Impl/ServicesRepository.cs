using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories.Impl
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly CleanersApiContext _context;

        public ServicesRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Service>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Service> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Service> Create(Service t)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(Service t)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}