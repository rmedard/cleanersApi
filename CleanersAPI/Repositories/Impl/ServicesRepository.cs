using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
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
            return _context.Services.SingleOrDefaultAsync(s => s.Id == id);
        }

        public bool DoesExist(int professionId)
        {
            return _context.Services.Any(p => p.Id == professionId);
        }

        public async Task<Service> Create(Service customer)
        {
            var newProfession = _context.Services.Add(customer).Entity;
            await _context.SaveChangesAsync();
            return newProfession;
        }

        public async Task<bool> Update(Service service)
        {
            if (!service.IsActive)
            {
                var expertises = _context.Expertises.Where(e => e.ServiceId.Equals(service.Id) && e.IsActive).ToList();
                foreach (var expertise in expertises)
                {
                    expertise.IsActive = false;
                    _context.Entry(expertise).Property(e => e.IsActive).IsModified = true;
                }
            }
            
            _context.Entry(service).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return false;
            }            
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var service = GetById(id);
            _context.Services.Remove(service.Result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Service>> GetServicesByCategory(Category category)
        {
            Console.WriteLine(DateTime.Now);
            return await _context.Services.Where(s => category.Equals(s.Category)).ToListAsync();
        }
    }
}
