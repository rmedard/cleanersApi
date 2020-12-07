using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly CleanersApiContext _context;

        public ReservationsRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .ThenInclude(c=> c.User)
                .Include(r => r.Expertise)
                .ThenInclude(e => e.Service)
                .Include(r => r.Expertise.Professional)
                .ThenInclude(p => p.User)
                .OrderBy(r => r.StartTime).ToListAsync();
        }

        public async Task<Reservation> GetById(int id)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .ThenInclude(c => c.User)
                .Include(r => r.Expertise.Service)
                .Include(r => r.Expertise.Professional)
                .ThenInclude(p => p.User)
                .SingleOrDefaultAsync(reservation => reservation.Id.Equals(id));
        }

        public async Task<IEnumerable<Reservation>> Search(ReservationSearchCriteria searchCriteria)
        {
            IQueryable<Reservation> queryable = _context.Reservations.Include(r => r.Expertise);
            
            if (searchCriteria.Professional != null)
            {
                queryable = queryable.Where(r => r.Expertise.ProfessionalId.Equals(searchCriteria.Professional.Id));
            }
            
            if(searchCriteria.Customer != null)
            {
                queryable = queryable.Where(r => r.CustomerId.Equals(searchCriteria.Customer.Id));
            }
            
            if(searchCriteria.Status != null)
            {
                queryable = queryable.Where(r => searchCriteria.Status.Equals(r.Status));
            }
            
            if(searchCriteria.DateTime != null)
            {
                queryable = queryable.Where(r => r.StartTime.DayOfYear.Equals(searchCriteria.DateTime.Value.DayOfYear) 
                                                 && r.StartTime.Year.Equals(searchCriteria.DateTime.Value.Year));
            }
            else
            {
                queryable = queryable.Where(r => r.EndTime.CompareTo(DateTime.Now) < 0);
            }

            if (searchCriteria.HasBill != null)
            {
                var hasBill = searchCriteria.HasBill.Value;
                queryable = queryable.Where(r => hasBill ? r.Billing != null : r.Billing == null);   
            }

            return await queryable
                .Include(r => r.Customer)
                .ThenInclude(c => c.User)
                .Include(r => r.Expertise)
                .ThenInclude(e => e.Service)
                .OrderByDescending(r => r.StartTime).ToListAsync();
        }

        public async Task GenerateUpcomingReservation()
        {
            //Fetch all active and future reservations
            var activeReservations = _context.Reservations.Where(r => r.Recurrence != null 
                                                                      && r.Recurrence.IsActive 
                                                                      && r.StartTime.CompareTo(DateTime.Now) > 0)
                .OrderByDescending(r => r.StartTime);
            
            //Group daily recurring reservations by customerId
            var dailyRecurrenceReservations = activeReservations
                .Where(r => r.Recurrence.Label.Equals("d")).ToList();

            var groupingDaily = dailyRecurrenceReservations.GroupBy(r => r.CustomerId);
            
            //Loop through customer reservations + create additional reservations
            foreach (var grouping in groupingDaily)
            {
                var dailyReservationsCount = grouping.Count();
                if (dailyReservationsCount >= 7) continue;
                var lastReservation = grouping.Last();
                while (dailyReservationsCount <= 7)
                {
                    var newReservation = new Reservation
                    {
                        Status = Status.Confirmed,
                        CustomerId = lastReservation.CustomerId,
                        ExpertiseId = lastReservation.ExpertiseId,
                        RecurrenceId = lastReservation.RecurrenceId,
                        StartTime = lastReservation.StartTime.AddDays(1),
                        EndTime = lastReservation.EndTime.AddDays(1),
                        TotalCost = lastReservation.TotalCost
                    };
                    lastReservation = await Create(newReservation);
                    ++dailyReservationsCount;
                }
            }

            //Group weekly recurring reservations by customerId
            var weeklyRecurrenceReservations = activeReservations
                .Where(r => r.Recurrence.Label.Equals("w")).ToList();
            var groupingWeekly = weeklyRecurrenceReservations.GroupBy(r => r.CustomerId);
            foreach (var grouping in groupingWeekly)
            {
                var weeklyRecurrenceCount = grouping.Count();
                if (weeklyRecurrenceCount < 3)
                {
                    var lastReservation = grouping.Last();
                    while (weeklyRecurrenceCount <= 3)
                    {
                        var newReservation = new Reservation
                        {
                            Status = Status.Confirmed,
                            CustomerId = lastReservation.CustomerId,
                            ExpertiseId = lastReservation.ExpertiseId,
                            RecurrenceId = lastReservation.RecurrenceId,
                            StartTime = lastReservation.StartTime.AddDays(7),
                            EndTime = lastReservation.EndTime.AddDays(7),
                            TotalCost = lastReservation.TotalCost
                        };
                        lastReservation = await Create(newReservation);
                        ++weeklyRecurrenceCount;
                    } 
                }   
            }
        }

        public bool DoesExist(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Reservation> Create(Reservation reservation)
        {
            var order = (await _context.Reservations.AddAsync(reservation)).Entity;
            await _context.SaveChangesAsync();
            return await GetById(order.Id);
        }

        public async Task<bool> Update(Reservation reservation)
        {
            _context.Entry(reservation).State = EntityState.Modified;
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
    }
}