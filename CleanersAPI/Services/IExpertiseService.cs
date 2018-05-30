using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IExpertiseService
    {
        Task<Expertise> FindExpertise(int professionalId, int professionId);
    }
}