using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ServicesService : CleanersService<Reservation>, IServicesService
    {

        private readonly IServicesRepository _servicesRepository;

        public ServicesService(IServicesRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        protected override ICleanersRepository<Reservation> GetRepository()
        {
            return _servicesRepository;
        }
    }
}