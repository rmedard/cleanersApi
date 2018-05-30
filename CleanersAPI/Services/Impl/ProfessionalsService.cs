using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ProfessionalsService : CleanersService<Professional>, IProfessionalsService
    {

        private readonly IProfessionalsRepository _professionalsRepository;

        public ProfessionalsService(IProfessionalsRepository repository)
        {
            _professionalsRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override ICleanersRepository<Professional> GetRepository()
        {
            return _professionalsRepository;
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

        public void OrderService(Expertise expertise)
        {
            throw new NotImplementedException();
        }

        public bool IsFree(DateTime dateTime, int numberOfHours)
        {
            throw new NotImplementedException();
        }

        public new Task<Professional> Create(Professional professional)
        {
            professional.RegNumber = "PRO_" + GenerateRegistrationNumber(10000, 90000);
            return _professionalsRepository.Create(professional);
        }
        
        private static int GenerateRegistrationNumber(int min, int max)  
        {  
            var random = new Random();  
            return random.Next(min, max);  
        }
    }
}
