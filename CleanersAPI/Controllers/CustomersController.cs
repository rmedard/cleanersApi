using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
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

//            var person = await _customersService.Customers.SingleOrDefaultAsync(m => m.Id == id);

            var customer = await _customersService.GetOneById(id);
            
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient([FromRoute] int id, [FromBody] Customer customer)
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

        // POST: api/People
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _customersService.Create(customer);

            return CreatedAtAction("GetCustomer", new { id = created.Id }, created);
        }

        // DELETE: api/People/5
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