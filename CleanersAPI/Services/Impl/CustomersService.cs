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
            customer.RegNumber = "CUST_" + GenerateRegistrationNumber(10000, 90000);
            return _customersRepository.Create(customer);
        }

        public Task<IEnumerable<Service>> getOrderedServices(int customerId)
        {
            throw new System.NotImplementedException();
        }
        
        private static int GenerateRegistrationNumber(int min, int max)  
        {  
            var random = new Random();  
            return random.Next(min, max);  
        }  
    }
}