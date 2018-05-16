using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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

        // PUT: api/People/5
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

        [HttpPost("{id}/user")]
        public IActionResult AddUser([FromRoute] int id, [FromBody] UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_authService.UserExists(userForLoginDto.Username).Result)
            {
                return BadRequest("Username already exists");
            }

            var customer = _customersService.GetOneById(id);
            if (customer == null)
            {
                return NotFound("Professional not found");
            }
            _authService.AddUserToCustomer(customer.Result, userForLoginDto);
            return NoContent();
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