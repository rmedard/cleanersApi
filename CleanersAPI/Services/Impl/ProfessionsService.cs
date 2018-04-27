using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ProfessionsService : IProfessionsService
    {

        private readonly IProfessionsRepository _professionsRepository;

        public ProfessionsService(IProfessionsRepository repository)
        {
            _professionsRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<IEnumerable<Profession>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Profession> GetOneById(int id)
        {
            throw new NotImplementedException();
        }

        public bool DoesExist(int professionId)
        {
            return _professionsRepository.DoesExist(professionId);
        }

        public Task<Profession> Create(Profession profession)
        {
            throw new NotImplementedException();
        }

        public Task<Profession> Update(Profession profession)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
