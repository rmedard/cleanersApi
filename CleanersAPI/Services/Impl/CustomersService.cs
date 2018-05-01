using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class CustomersService : ICustomersService
    {

        private readonly ICustomersRepository _customersRepository;

        public CustomersService(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository ?? throw new ArgumentNullException(nameof(customersRepository));
        }

        public Task<IEnumerable<Customer>> GetAll()
        {
            return _customersRepository.GetAll();
        }

        public Task<Customer> GetOneById(int id)
        {
            return _customersRepository.GetById(id);
        }

        public bool DoesExist(int id)
        {
            return _customersRepository.DoesExist(id);
        }

        public Task<Customer> Create(Customer customer)
        {
            customer.RegNumber = "CUST_" + GenerateRegistrationNumber(10000, 90000);
            return _customersRepository.Create(customer);
        }

        public Task<bool> Update(Customer customer)
        {
            return _customersRepository.Update(customer);
        }

        public Task<bool> Delete(int id)
        {
            return _customersRepository.Delete(id);
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