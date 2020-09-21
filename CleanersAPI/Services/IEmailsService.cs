using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IEmailsService : ICleanersService<Email>
    {
        Task notifyUsersOnReservationCreation(Reservation reservation);
    }
}