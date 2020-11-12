using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using IronPdf;

namespace CleanersAPI.Services
{
    public interface IBillingsService : ICleanersService<Billing>
    {
        Task<Billing> Create(ReservationSearchCriteria reservationSearchCriteria);

        Task<PdfDocument> GenerateInvoice(User person, IEnumerable<Reservation> reservations);
    }
}