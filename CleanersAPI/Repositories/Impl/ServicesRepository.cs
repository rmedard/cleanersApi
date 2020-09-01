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

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations.ToListAsync();
        }

        public Task<Reservation> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Reservation> Create(Reservation t)
        {
            var order = _context.Reservations.Add(t).Entity;
            await _context.SaveChangesAsync();
            return order;
        }

        public Task<bool> Update(Reservation t)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}