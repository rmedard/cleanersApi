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

        public ProfessionalsService(IProfessionalsRepository professionalsRepository)
        {
            _professionalsRepository = professionalsRepository ??
                                       throw new ArgumentNullException(nameof(professionalsRepository));
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

        public Task<IEnumerable<Reservation>> GetOrders(int professionalId)
        {
            return _professionalsRepository.GetOrders(professionalId);
        }

        public Task<IEnumerable<Service>> GetProfessions(int professionalId)
        {
            return _professionalsRepository.GetServices(professionalId);
        }

        public void OrderService(Expertise expertise)
        {
            throw new NotImplementedException();
        }

        public bool IsFree(int professionalId, DateTime dateTime, int numberOfHours)
        {
            var starTime = dateTime;
            var endTime = dateTime.AddHours(numberOfHours);

            return !_professionalsRepository.GetOrders(professionalId).Result.Any(serv =>
                       DateTime.Compare(serv.StartTime, endTime) < 0 &&
                       DateTime.Compare(endTime, starTime) > 0 &&
                       serv.Status == Status.Confirmed);
        }

        public new Task<Professional> Create(Professional professional)
        {
            return _professionalsRepository.Create(professional);
        }

        private static int GenerateRegistrationNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }
    }
}