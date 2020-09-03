using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IBillingsService : ICleanersService<Billing>
    {
        Task<Billing> Create(ReservationSearchCriteria reservationSearchCriteria);
    }
}