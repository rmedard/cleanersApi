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
        Task<IEnumerable<Service>> GetOrders(int professionalId); 
        Task<IEnumerable<Profession>> GetProfessions(int professionalId);
        void OrderService(Expertise expertise);
        bool IsFree(DateTime dateTime, int numberOfHours);
    }
}
