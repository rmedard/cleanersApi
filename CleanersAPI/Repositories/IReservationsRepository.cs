using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IReservationsRepository : ICleanersRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> Search(ReservationSearchCriteria searchCriteria);
    }
}