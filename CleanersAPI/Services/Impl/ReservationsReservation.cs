using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ReservationsReservation : CleanersService<Reservation>, IReservationsService
    {

        private readonly IReservationsRepository _reservationsRepository;

        public ReservationsReservation(IReservationsRepository reservationsRepository)
        {
            _reservationsRepository = reservationsRepository;
        }

        protected override ICleanersRepository<Reservation> GetRepository()
        {
            return _reservationsRepository;
        }

        public Task<IEnumerable<Reservation>> searchByProfessionalByStatus(Professional professional, Status status)
        {
            ReservationSearchCriteria searchCriteria = new ReservationSearchCriteria().build(professional).build(status);
            return _reservationsRepository.GetBySearchCriteria(searchCriteria);
        }
    }
}