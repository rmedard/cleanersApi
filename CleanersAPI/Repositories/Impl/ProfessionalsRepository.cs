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
            return await _context.Professionals
                .Include(prof => prof.Address)
                .Include(prof => prof.Expertises)
                .ThenInclude(e => e.Service)
                .ToListAsync();
        }

        public Task<Professional> GetById(int id)
        {
            return _context.Professionals
                .Include(prof => prof.Address)
                .Include(p => p.Expertises)
                .SingleOrDefaultAsync(prof => prof.Id.Equals(id));
        }

        public void GrantExpertise(Expertise expertise)
        {
            _context.Expertises.Add(expertise);
            _context.SaveChanges();
        }

        public void UpdateExpertise(Expertise expertise)
        {
            _context.Entry(expertise).State = EntityState.Modified;
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

        public bool IsFree(int professionalId, DateTime dateTime, int numberOfHours)
        {
            var starTime = dateTime;
            var endTime = dateTime.AddHours(numberOfHours);
            return !_context.Reservations
                .Any(r => r.Expertise.ProfessionalId.Equals(professionalId)
                          && DateTime.Compare(r.StartTime, endTime) < 0
                          && DateTime.Compare(r.EndTime, starTime) > 0
                          && Status.Confirmed.Equals(r.Status));
        }

        public async Task<Professional> GetProfessionalByUserId(int userId)
        {
            return await _context.Professionals.FirstOrDefaultAsync(p => userId.Equals(p.UserId));
        }

        public async Task<IEnumerable<Professional>> GetAvailable(AvailabilityFinder availabilityFinder)
        {
            var startTime = availabilityFinder.DateTime;
            var endTime = availabilityFinder.DateTime.AddHours(availabilityFinder.Duration);
            return await _context.Professionals
                .Where(p => !_context.Reservations
                    .Where(r => DateTime.Compare(r.StartTime, endTime) < 0
                                && DateTime.Compare(r.EndTime, startTime) > 0)
                    .Select(r => r.Expertise.ProfessionalId).Contains(p.Id))
                .Where(p => p.Expertises.Where(e => e.Active)
                    .Select(e => e.ServiceId).Contains(availabilityFinder.ServiceId))
                .Include(p => p.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expertise>> GetExpertises(int professionalId)
        {
            return await _context.Expertises.Include(e => e.Service)
                .Where(e => e.ProfessionalId.Equals(professionalId)).ToListAsync();
        }

        public bool DoesExist(int professionalId)
        {
            return _context.Professionals.Any(e => e.Id == professionalId);
        }

        public async Task<Professional> Create(Professional professional)
        {
            var newProfessional = (await _context.Professionals.AddAsync(professional)).Entity;
            await _context.SaveChangesAsync();
            return newProfessional;
        }

        public async Task<bool> Update(Professional professional)
        {
            _context.Entry(professional).State = EntityState.Modified;
            _context.Entry(professional.Address).State = EntityState.Modified;
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

        public async Task<bool> Delete(int professionalId)
        {
            var professional = GetById(professionalId);
            _context.Professionals.Remove(professional.Result);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}