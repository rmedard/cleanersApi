using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IProfessionsRepository
    {
        IEnumerable<Profession> GetAllProfessionals();
        Task<Profession> GetOneById(int id);
        bool DoesExist(int id);
        Task<Profession> Create(Profession profession);
        Task<Profession> Update(Profession profession);
        Task<bool> Delete(int id);
    }
}
