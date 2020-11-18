using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
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

        public Task<IEnumerable<Expertise>> GetExpertises(int professionalId)
        {
            return _professionalsRepository.GetExpertises(professionalId);
        }

        public void OrderService(Expertise expertise)
        {
            throw new NotImplementedException();
        }

        public bool IsFree(int professionalId, DateTime dateTime, int numberOfHours)
        {
            return _professionalsRepository.IsFree(professionalId, dateTime, numberOfHours);
        }

        public async Task<Professional> GetProfessionalByUserId(int userId)
        {
            return await _professionalsRepository.GetProfessionalByUserId(userId);
        }

        public new Task<Professional> Create(Professional professional)
        {
            return _professionalsRepository.Create(professional);
        }
    }
}