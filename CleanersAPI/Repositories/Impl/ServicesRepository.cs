using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly CleanersApiContext _context;

        public ServicesRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAll()
        {
            return await _context.Services.ToListAsync();
        }

        public Task<Service> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Service> Create(Service t)
        {
            var order = _context.Services.Add(t).Entity;
            await _context.SaveChangesAsync();
            return order;
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