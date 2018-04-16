using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public class ProfessionsRepository : IProfessionsRepository
    {

        private readonly CleanersApiContext _context;

        public ProfessionsRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public IEnumerable<Profession> GetAllProfessionals()
        {
            throw new NotImplementedException();
        }

        public Task<Profession> GetOneById(int id)
        {
            throw new NotImplementedException();
        }

        public bool DoesExist(int professionId)
        {
            return _context.Professions.Any(p => p.Id == professionId);
        }

        public Task<Profession> Create(Profession profession)
        {
            throw new NotImplementedException();
        }

        public Task<Profession> Update(Profession profession)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
