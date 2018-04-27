using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IProfessionalsRepository : ICleanersRepository<Professional>
    {
        Task<IEnumerable<Profession>> GetProfessions(int professionalId);
        void GrantExpertise(int professionalId, int professionId);
    }
}
