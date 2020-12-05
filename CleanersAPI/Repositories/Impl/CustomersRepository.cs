using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class CustomersRepository : ICustomersRepository
    {
        
        private readonly CleanersApiContext _context;

        public CustomersRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers
                .Include(c => c.User)
                .ThenInclude(u => u.Address).ToListAsync();
        }

        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers
                .Include(c => c.User.Address)
                .Include(c => c.User.Roles)
                .Include(c => c.Reservations)
                .ThenInclude(r => r.Expertise.Service)
                .Include(c => c.Reservations)
                .ThenInclude(r => r.Expertise.Professional)
                .ThenInclude(p => p.User)
                .Include(d => d.Reservations)
                .ThenInclude(r => r.Recurrence)
                .Include(d => d.Reservations)
                .ThenInclude(r => r.Billing)
                .SingleOrDefaultAsync(customer => customer.Id == id);
        }

        public bool DoesExist(int id)
        {
            return _context.Customers.Any(customer => customer.Id == id);
        }

        public async Task<Customer> Create(Customer customer)
        {
            var saved = (await _context.Customers.AddAsync(customer)).Entity;
            await _context.SaveChangesAsync();
            return saved;
        }

        public async Task<bool> Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            _context.Entry(customer.User).State = EntityState.Modified;
            _context.Entry(customer.User.Address).State = EntityState.Modified;
            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return false;
            }
            
        }

        public async Task<bool> Delete(int id)
        {
            var customer = _context.Customers.SingleOrDefault(cust => cust.Id == id);
            try
            {
                if (customer == null) return false;
                _context.Entry(customer).State = EntityState.Deleted;
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public async Task<IEnumerable<Reservation>> GetCustomerOrderedServices(int customerId)
        {
            return await _context.Reservations.Where(s => s.CustomerId == customerId).ToListAsync();
        }

        public async Task<Customer> GetCustomerByUserId(int userId)
        {
            return await _context.Customers
                .Include(c => c.User)
                .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(c => c.UserId.Equals(userId));
        }

        public async Task<IEnumerable<Customer>> GetAvailableBillableCustomers()
        {
            return await _context.Customers
                .Where(c => c.Reservations
                    .Any(r => r.Billing == null && r.EndTime.CompareTo(DateTime.Now) < 0))
                .ToListAsync();
        }
    }
}