using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IProfessionalsRepository
    {
        IEnumerable<Professional> GetAllProfessionals();
        Task<Professional> GetOneById(int id);
        IEnumerable<Profession> GetProfessions(int professionalId);
        void GrantExpertise(int professionalId, int professionId);
        bool DoesExist(int id);
        Task<Professional> Create(Professional professional);
        bool Update(Professional professional);
        bool Delete(int id);
    }
}
