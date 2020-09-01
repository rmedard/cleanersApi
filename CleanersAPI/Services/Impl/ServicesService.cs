using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ServicesService : CleanersService<Service>, IServicesService
    {
        private readonly IServicesRepository _servicesRepository;

        public ServicesService(IServicesRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }
        
        protected override ICleanersRepository<Service> GetRepository()
        {
            return _servicesRepository;
        }
    }
}