using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class CustomersService : CleanersService<Customer>, ICustomersService
    {

        private readonly ICustomersRepository _customersRepository;

        public CustomersService(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository ?? throw new ArgumentNullException(nameof(customersRepository));
        }

        protected override ICleanersRepository<Customer> GetRepository()
        {
            return _customersRepository;
        }

        public new Task<Customer> Create(Customer customer)
        {
            return _customersRepository.Create(customer);
        }

        public Task<IEnumerable<Reservation>> getOrderedServices(int customerId)
        {
            throw new System.NotImplementedException();
        }
    }
}