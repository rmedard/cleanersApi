using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IProfessionalsService
    {
        IEnumerable<Professional> GetAllProfessionals();
        Task<Professional> GetOneById(int id);
        void GrantExpertise(int professionalId, int professionId);
        IEnumerable<Profession> GetProfessions(int professionalId);
        bool DoesExist(int id);
        Task<Professional> Create(Professional professional);
        bool Update(Professional professional);
        bool Delete(int id);
    }
}
