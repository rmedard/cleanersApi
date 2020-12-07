using System.Threading.Tasks;
using CleanersAPI.Models;
using SendGrid;

namespace CleanersAPI.Services
{
    public interface IEmailsService : ICleanersService<Email>
    {
        public Task<Response> SendEmail(Email email);
        public Task<Response> SendEmail(Email email, string filename, string attachmentInBase64);
        Task notifyUsersOnReservationCreation(Reservation reservation);
        Task notifyUsersOnReservationCancellation(Reservation reservation);
    }
}