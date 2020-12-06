using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class BillingsRepository : IBillingsRepository
    {
        private readonly CleanersApiContext _context;
        
        public BillingsRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Billing>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Billing> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Billing> Create(Billing billing)
        {
            var saved = (await _context.Billings.AddAsync(billing)).Entity;
            await _context.SaveChangesAsync();
            return saved;
        }

        public Task<bool> Update(Billing t)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Billing> Create(Billing billing, IEnumerable<Reservation> reservations)
        {
            var newBilling = (await _context.Billings.AddAsync(billing)).Entity;
            foreach (var reservation in reservations)
            {
                reservation.Billing = newBilling;
                _context.Reservations.Update(reservation);
            }
            await _context.SaveChangesAsync();
            return newBilling;
        }

        public async Task<IEnumerable<Billing>> GetBillings()
        {
            return await _context.Billings
                .Include(b => b.Reservations)
                .ThenInclude(r => r.Expertise)
                .ThenInclude(e => e.Service)
                .Include(b => b.Reservations)
                .ThenInclude(r => r.Customer)
                .ThenInclude(c => c.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Billing>> GetBillings(int customerId)
        {
            return await _context.Billings
                .Include(b => b.Reservations)
                .Where(b => b.Reservations.Any(r => r.CustomerId.Equals(customerId)))
                .ToListAsync();
        }
    }
}