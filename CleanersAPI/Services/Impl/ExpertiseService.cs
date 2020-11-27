using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ExpertiseService : CleanersService<Expertise>, IExpertiseService
    {

        private readonly IExpertiseRepository _expertiseRepository;

        public ExpertiseService(IExpertiseRepository expertiseRepository)
        {
            _expertiseRepository = expertiseRepository;
        }

        public Task<Expertise> FindExpertise(int professionalId, int professionId)
        {
            return _expertiseRepository.GetOne(professionalId, professionId);
        }

        public async Task<IEnumerable<Expertise>> GetExpertisesByServiceId(int serviceId)
        {
            return await _expertiseRepository.GetExpertisesByServiceId(serviceId);
        }

        public IEnumerable<Expertise> GetAvailableExpertises(AvailabilityFinder availabilityFinder)
        {
            return _expertiseRepository.GetAvailable(availabilityFinder);
        }

        protected override ICleanersRepository<Expertise> GetRepository()
        {
            return _expertiseRepository;
        }
    }
}