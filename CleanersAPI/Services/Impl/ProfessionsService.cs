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
            return _professionsRepository.GetAll();
        }

        public Task<Profession> GetOneById(int id)
        {
            return _professionsRepository.GetById(id);
        }

        public bool DoesExist(int professionId)
        {
            return _professionsRepository.DoesExist(professionId);
        }

        public Task<Profession> Create(Profession profession)
        {
            return _professionsRepository.Create(profession);
        }

        public Task<bool> Update(Profession profession)
        {
            return _professionsRepository.Update(profession);
        }

        public Task<bool> Delete(int id)
        {
            return _professionsRepository.Delete(id);
        }
    }
}
