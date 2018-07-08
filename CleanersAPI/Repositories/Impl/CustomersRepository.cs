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
            return await _context.Customers.Include(c => c.Address).ToListAsync();
        }

        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.orders).ThenInclude(o => o.Expertise).ThenInclude(e => e.Profession)
                .FirstOrDefaultAsync(customer => customer.Id == id);
        }

        public bool DoesExist(int id)
        {
            return _context.Customers.Any(customer => customer.Id == id);
        }

        public async Task<Customer> Create(Customer customer)
        {
            var saved = _context.Customers.Add(customer).Entity;
            await _context.SaveChangesAsync();
            return saved;
        }

        public async Task<bool> Update(Customer customer)
        {
//            _context.Entry(customer).State = EntityState.Modified;
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

        public async Task<IEnumerable<Service>> getCustomerOrderedServices(int customerId)
        {
            return await _context.Services.Where(s => s.CustomerId == customerId).ToListAsync();
        }
    }
}