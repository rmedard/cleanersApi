using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
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
            return await _context.Reservations.SingleOrDefaultAsync(reservation => reservation.Id.Equals(id));
        }

        public async Task<IEnumerable<Reservation>> Search(ReservationSearchCriteria searchCriteria)
        {
            var queryable = _context.Reservations.AsNoTracking();
            
            if (searchCriteria.Professional != null)
            {
                queryable = queryable.Where(r => r.Expertise.ProfessionalId.Equals(searchCriteria.Professional.Id));
            }
            else if(searchCriteria.Customer != null)
            {
                queryable = queryable.Where(r => r.Customer.Id.Equals(searchCriteria.Customer.Id));
            }
            else if(searchCriteria.Status != null)
            {
                queryable = queryable.Where(r => r.Status.Equals(searchCriteria.Status));
            }
            else if(searchCriteria.DateTime != null)
            {
                queryable = queryable.Where(r =>
                    r.StartTime.ToShortDateString().Equals(searchCriteria.DateTime.Value.ToShortDateString()));
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
            return order;
        }

        public Task<bool> Update(Reservation reservation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}