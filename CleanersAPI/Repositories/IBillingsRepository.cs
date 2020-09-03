using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IBillingsRepository : ICleanersRepository<Billing>
    {
        Task<Billing> Create(Billing billing, IEnumerable<Reservation> reservations);
    }
}