using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class BillingsService : IBillingsService
    {
        private readonly IBillingsRepository _billingsRepository;
        private readonly IReservationsRepository _reservationsRepository;


        public BillingsService(IBillingsRepository billingsRepository, IReservationsRepository reservationsRepository)
        {
            _billingsRepository = billingsRepository;
            _reservationsRepository = reservationsRepository;
        }

        public Task<IEnumerable<Billing>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Billing> GetOneById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Billing> Create(Billing t)
        {
            throw new System.NotImplementedException();
        }

        public Task<Billing> Create(ReservationSearchCriteria reservationSearchCriteria)
        {
            var reservations = _reservationsRepository.Search(reservationSearchCriteria).Result.ToList();
            if (!reservations.Any()) return null;
            var billing = new Billing {Date = DateTime.Now, TotalPrice = reservations.Sum(r => r.TotalCost)};
            return _billingsRepository.Create(billing, reservations);
        }

        public Task<bool> Update(Billing t)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}