using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IReservationsService : ICleanersService<Reservation>
    {
        Task<IEnumerable<Reservation>> Search(ReservationSearchCriteria reservationSearchCriteria);

        new Task<Reservation> GetOneById(int reservationId);
    }
}