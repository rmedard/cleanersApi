using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class ProfessionsService : CleanersService<Service>, IProfessionsService
    {

        private readonly IProfessionsRepository _professionsRepository;

        public ProfessionsService(IProfessionsRepository repository)
        {
            _professionsRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override ICleanersRepository<Service> GetRepository()
        {
            return _professionsRepository;
        }
    }
}
