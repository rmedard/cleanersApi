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

        public Task<Reservation> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reservation>> GetByCustomer(int customerId)
        {
            return await _context.Reservations.Where(r => r.Customer.Id == customerId).ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByCustomerByStatus(int customerId, Status status)
        {
            return await _context.Reservations
                .Where(r => r.Customer.Id == customerId && r.Status.Equals(status))
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByProfessional(int professionalId)
        {
            return await _context.Reservations.Where(r => r.Expertise.ProfessionalId == professionalId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Reservation>> GetByProfessionalByStatus(int professionalId, Status status)
        {
            return await _context.Reservations
                .Where(r => r.Expertise.ProfessionalId == professionalId && r.Status.Equals(status))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Reservation>> GetBySearchCriteria(ReservationSearchCriteria searchCriteria)
        {
            DbSet<Reservation> queryable = _context.Reservations;
            
            if (searchCriteria._professional != null)
            {
                queryable.Where(r => r.Expertise.ProfessionalId.Equals(searchCriteria._professional.Id));
            }
            else if(searchCriteria._customer != null)
            {
                queryable.Where(r => r.Customer.Id.Equals(searchCriteria._customer.Id));
            }
            else if(searchCriteria._status != null)
            {
                queryable.Where(r => r.Status.Equals(searchCriteria._status));
            }
            else if(searchCriteria._dateTime != null)
            {
                // queryable.Where()
            }
            return await queryable.ToListAsync();
        }

        public bool DoesExist(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Reservation> Create(Reservation t)
        {
            var order = _context.Reservations.Add(t).Entity;
            await _context.SaveChangesAsync();
            return order;
        }

        public Task<bool> Update(Reservation t)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}