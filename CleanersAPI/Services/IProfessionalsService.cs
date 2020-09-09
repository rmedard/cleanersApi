using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IProfessionalsService : ICleanersService<Professional>
    {
        void GrantExpertise(Expertise expertise);
        void UpdateExpertise(Expertise expertise);
        Task<IEnumerable<Expertise>> GetExpertises(int professionalId);
        void OrderService(Expertise expertise);
        bool IsFree(int professionalId, DateTime dateTime, int numberOfHours);
        
        Task<Professional> GetProfessionalByUserId(int userId);
    }
}
