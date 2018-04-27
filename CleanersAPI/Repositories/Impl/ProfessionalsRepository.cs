using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using SQLitePCL;

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
            return await _context.Professionals.Include(prof => prof.User).Include(prof => prof.Address).ToListAsync();
        }

        public Task<Professional> GetById(int id)
        {
            return _context.Professionals.Include(prof => prof.User).Include(prof => prof.Address)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public void GrantExpertise(int professionalId, int professionId)
        {
            var professional = _context.Professionals.Include(prof => prof.Expertises).Single(prof => prof.Id == professionalId);
            var profession = _context.Professions.Find(professionId);
            if (professional.Expertises.Any(exp => exp.ProfessionId == professionId))
            {
                return;
            }
            professional.Expertises.Add(new Expertise { Profession = profession, Professional = professional });
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Profession>> GetProfessions(int professionalId)
        {
            var professional = _context.Professionals.Include(p => p.Expertises).ThenInclude(exp => exp.Profession).Single(prof => prof.Id == professionalId);
            return professional.Expertises.Select(expertise => expertise.Profession).ToList();
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

        public Task<Professional> Update(Professional professional)
        {
            _context.Entry(professional).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return null;
            }
            
            return null;
        }

        public bool Delete(int professionalId)
        {
            throw new NotImplementedException();
        }
    }
}