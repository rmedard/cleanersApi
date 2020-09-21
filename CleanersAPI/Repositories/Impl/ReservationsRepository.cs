using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using Microsoft.Data.SqlClient.Server;
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
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetById(int id)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Expertise.Professional)
                .Include(r => r.Expertise.Service)
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

            if (searchCriteria.HasBill != null)
            {
                var hasBill = searchCriteria.HasBill.Value;
                queryable = queryable.Where(r => hasBill ? r.BillingId != null : r.BillingId == null);   
            }

            return await queryable.ToListAsync();
        }

        public bool DoesExist(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Reservation> Create(Reservation customer)
        {
            var order = (await _context.Reservations.AddAsync(customer)).Entity;
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