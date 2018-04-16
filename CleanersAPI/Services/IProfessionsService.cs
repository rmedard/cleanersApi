using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IProfessionsService
    {
        IEnumerable<Profession> GetAllProfessionals();
        Task<Profession> GetOneById(int id);
        bool DoesExist(int id);
        Task<Profession> Create(Profession profession);
        Task<Profession> Update(Profession profession);
        bool Delete(int id);
    }
}
