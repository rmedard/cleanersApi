using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IProfessionalsRepository : ICleanersRepository<Professional>
    {
        Task<IEnumerable<Service>> GetProfessions(int professionalId);
        Task<IEnumerable<Reservation>> GetOrders(int professionalId);
        void GrantExpertise(Expertise expertise);
        void UpdateExpertise(Expertise expertise);
    }
}
