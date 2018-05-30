using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ExpertiseService : IExpertiseService
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
    }
}