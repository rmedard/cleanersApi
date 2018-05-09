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

        public void GrantExpertise(Expertise expertise)
        {
            _professionalsRepository.GrantExpertise(expertise);
        }

        public void UpdateExpertise(Expertise expertise)
        {
            _professionalsRepository.UpdateExpertise(expertise);
        }

        public Task<IEnumerable<Service>> GetOrders(int professionalId)
        {
            return _professionalsRepository.GetOrders(professionalId);
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
            professional.RegNumber = "PRO_" + GenerateRegistrationNumber(10000, 90000);
            return _professionalsRepository.Create(professional);
        }

        public Task<bool> Update(Professional professional)
        {
            return _professionalsRepository.Update(professional);
        }

        public Task<bool> Delete(int professionalId)
        {
            return _professionalsRepository.Delete(professionalId);
        }
        
        private static int GenerateRegistrationNumber(int min, int max)  
        {  
            var random = new Random();  
            return random.Next(min, max);  
        }
    }
}
