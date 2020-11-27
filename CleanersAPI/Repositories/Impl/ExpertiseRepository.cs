using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class ExpertiseRepository : IExpertiseRepository
    {
        private readonly CleanersApiContext _context;

        public ExpertiseRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expertise>> GetAll()
        {
            return await _context.Expertises
                .Include(e => e.Service)
                .Include(e => e.Professional)
                .ToListAsync();
        }

        public async Task<Expertise> GetById(int id)
        {
            return await _context.Expertises
                .Include(e => e.Service)
                .Include(e => e.Professional)
                .Include(e => e.Reservations)
                .FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public bool DoesExist(int id)
        {
            return _context.Expertises.Any(e => e.Id == id);
        }

        public Task<Expertise> Create(Expertise customer)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Expertise expertise)
        {
            _context.Entry(expertise).State = EntityState.Modified;
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

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Expertise> GetOne(int professionalId, int serviceId)
        {
            return await _context.Expertises.FirstOrDefaultAsync(exp =>
                exp.ProfessionalId == professionalId && exp.ServiceId == serviceId);
        }

        public async Task<IEnumerable<Expertise>> GetExpertisesByServiceId(int serviceId)
        {
            return await _context.Expertises
                .Include(e => e.Professional)
                .ThenInclude(p => p.User)
                .ThenInclude(u => u.Address)
                .Where(e => e.IsActive && e.ServiceId.Equals(serviceId)).ToListAsync();
        }

        public IEnumerable<Expertise> GetAvailable(AvailabilityFinder availabilityFinder)
        {
            var startTime = availabilityFinder.DateTime;
            var endTime = availabilityFinder.DateTime.AddHours(availabilityFinder.Duration);
            var sunday = DateTime.Parse("Jan 4, 1970");

            var queryable = _context.Expertises
                    .Include(e => e.Reservations)
                    .ThenInclude(r => r.Recurrence)
                    .Where(e => !_context.Reservations.Any(r => r.Recurrence == null
                                                                && e.ProfessionalId.Equals(r.Expertise.ProfessionalId)
                                                                && r.StartTime.CompareTo(endTime) < 0
                                                                && r.EndTime.CompareTo(startTime) > 0))
                    .Where(e => !_context.Reservations.Any(r => r.Recurrence != null
                                                                && r.Recurrence.IsActive
                                                                && e.ProfessionalId.Equals(r.Expertise.ProfessionalId)
                                                                && startTime.Hour.CompareTo(r.EndTime.Hour) < 0
                                                                && endTime.Hour.CompareTo(r.StartTime.Hour) > 0
                                                                && endTime.CompareTo(r.StartTime) > 0
                                                                && ("d".Equals(r.Recurrence.Label) ||
                                                                    "w".Equals(r.Recurrence.Label)
                                                                    && EF.Functions.DateDiffDay(sunday, r.StartTime.Date) %
                                                                    7 ==
                                                                    (int) availabilityFinder.DateTime.DayOfWeek)))
                    .Include(e => e.Professional)
                    .ThenInclude(p => p.User)
                    .ThenInclude(u => u.Address);
            if (availabilityFinder.Order == null) return queryable.ToList().FindAll(x => x.ServiceId.Equals(availabilityFinder.ServiceId));
            {
                return availabilityFinder.Order.Equals(Order.Descendant) ? 
                    queryable.OrderByDescending(e => e.HourlyRate).ToList().FindAll(x => x.ServiceId.Equals(availabilityFinder.ServiceId)) : 
                    queryable.OrderBy(e => e.HourlyRate).ToList().FindAll(x => x.ServiceId.Equals(availabilityFinder.ServiceId));
            }
        }
    }
}