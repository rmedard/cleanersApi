using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class ProfessionalsRepository : IProfessionalsRepository
    {
        private readonly CleanersApiContext _context;

        public ProfessionalsRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Professional>> GetAll()
        {
            return await _context.Professionals.Include(prof => prof.Address)
                .Include(prof => prof.Expertises)
                .ThenInclude(exp => exp.Service).ToListAsync();
        }

        public Task<Professional> GetById(int id)
        {
            return _context.Professionals.Include(prof => prof.Address).Include(prof => prof.Expertises).ThenInclude(ex => ex.Service)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetOrders(int professionalId)
        {
            return await _context.Services.Include(serv => serv.Expertise).Where(s => s.Expertise.ProfessionalId == professionalId).ToListAsync();
        }

        public void GrantExpertise(Expertise expertise)
        {
            _context.Expertises.Add(expertise);
            _context.SaveChanges();
        }

        public void UpdateExpertise(Expertise expertise)
        {
            _context.Entry(expertise).State = EntityState.Modified;
            Console.WriteLine("Yahinduwe: " + expertise);
            try
            {
                _context.SaveChangesAsync();    
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                throw;
            }
                 
        }

        public async Task<IEnumerable<Service>> GetProfessions(int professionalId)
        {
            var professional = await _context.Professionals.Include(p => p.Expertises).ThenInclude(exp => exp.Service).SingleAsync(prof => prof.Id == professionalId);
            return professional.Expertises.Select(expertise => expertise.Service).ToList();
        }

        public bool DoesExist(int professionalId)
        {
            return _context.Professionals.Any(e => e.Id == professionalId);
        }

        public async Task<Professional> Create(Professional professional)
        {
            var newProfessional = _context.Professionals.Add(professional).Entity;
            await _context.SaveChangesAsync();
            return newProfessional;
        }

        public async Task<bool> Update(Professional professional)
        {
            _context.Entry(professional).State = EntityState.Modified;
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

        public Task<bool> Delete(int professionalId)
        {
            throw new NotImplementedException();
        }
    }
}