using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ReservationsService : CleanersService<Reservation>, IReservationsService
    {

        private readonly IReservationsRepository _reservationsRepository;

        public ReservationsService(IReservationsRepository reservationsRepository)
        {
            _reservationsRepository = reservationsRepository;
        }

        protected override ICleanersRepository<Reservation> GetRepository()
        {
            return _reservationsRepository;
        }

        public Task<IEnumerable<Reservation>> Search(ReservationSearchCriteria reservationSearchCriteria)
        {
            return _reservationsRepository.Search(reservationSearchCriteria);
        }

        public Task GenerateUpcomingReservation()
        {
            return _reservationsRepository.GenerateUpcomingReservation();
        }
    }
}