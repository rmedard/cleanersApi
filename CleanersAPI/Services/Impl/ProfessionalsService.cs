using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ProfessionalsService : IProfessionalsService
    {

        private readonly IProfessionalsRepository _professionalsRepository;

        public ProfessionalsService(IProfessionalsRepository repository)
        {
            _professionalsRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<IEnumerable<Professional>> GetAll()
        {
            return _professionalsRepository.GetAll();
        }

        public Task<Professional> GetOneById(int id)
        {
            return _professionalsRepository.GetById(id);
        }

        public void GrantExpertise(int professionalId, int professionId)
        {
            _professionalsRepository.GrantExpertise(professionalId, professionId);
        }

        public Task<IEnumerable<Profession>> GetProfessions(int professionalId)
        {
            return _professionalsRepository.GetProfessions(professionalId);
        }

        public bool DoesExist(int professionalId)
        {
            return _professionalsRepository.DoesExist(professionalId);
        }

        public Task<Professional> Create(Professional professional)
        {
            return _professionalsRepository.Create(professional);
        }

        public Task<Professional> Update(Professional professional)
        {
            return _professionalsRepository.Update(professional);
        }

        public bool Delete(int professionalId)
        {
            return _professionalsRepository.Delete(professionalId);
        }
    }
}
