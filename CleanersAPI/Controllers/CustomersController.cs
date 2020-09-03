using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.User;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly ICustomersService _customersService;
        private readonly IAuthService _authService;

        public CustomersController(ICustomersService customersService, IAuthService authService)
        {
            _customersService = customersService;
            _authService = authService;
        }

        [HttpGet]
        public Task<IEnumerable<Customer>> GetCustomers()
        {
            return _customersService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customersService.GetOneById(id);
            
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id || !_customersService.DoesExist(id))
            {
                return NotFound();
            }

            var updated = await _customersService.Update(customer);
            if (updated)
            {
                return Ok(customer);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerForCreate customerForCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_authService.UserExists(customerForCreate.Customer.Email).Result)
            {
                return BadRequest("Customer already exists. Please login!!");
            }
            
            customerForCreate.Customer.User = _authService.GenerateUserAccount(customerForCreate.Customer, customerForCreate.Password);
            var newCustomer= await _customersService.Create(customerForCreate.Customer);

            return CreatedAtAction("GetCustomer", new { id = newCustomer.Id }, newCustomer);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_customersService.DoesExist(id))
            {
                return NotFound();
            }

            if (_customersService.Delete(id).Result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("{id}/services")]
        public async Task<IActionResult> GetCustomerOrderedServices([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (!_customersService.DoesExist(id))
            {
                return NotFound("Customer not found");
            }

            return Ok(await _customersService.getOrderedServices(id));
        }
    }
}