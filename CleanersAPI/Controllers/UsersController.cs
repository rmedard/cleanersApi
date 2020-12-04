using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class UsersController : Controller
    {

        private readonly IUsersService _usersService;
        private readonly ICustomersService _customersService;
        private readonly IProfessionalsService _professionalsService;

        public UsersController(IUsersService usersService, ICustomersService customersService, IProfessionalsService professionalsService)
        {
            _usersService = usersService;
            _customersService = customersService;
            _professionalsService = professionalsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return Ok(await _usersService.GetAll());
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid || !id.Equals(user.Id))
            {
                return BadRequest(ModelState);
            }
            
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var loggedInUser = await _usersService.GetOneById(loggedInUserId);
            if (!loggedInUserId.Equals(user.Id) && !loggedInUser.Roles.Any(r => r.Role.RoleName.Equals(RoleName.Admin)))
            {
                return StatusCode(403, "You do not have permission to update this user");
            }

            if (!user.IsActive)
            {
                var customer = _customersService.GetCustomerByUserId(user.Id).Result;
                if (customer != null && customer.IsActive)
                {
                    customer.IsActive = false;
                    await _customersService.Update(customer);
                }
            
                var professional = _professionalsService.GetProfessionalByUserId(user.Id).Result;
                if (professional != null && professional.IsActive)
                {
                    professional.IsActive = false;
                    await _professionalsService.Update(professional);
                }
            }

            var updated = await _usersService.Update(user);
            if (!updated)
            {
                return BadRequest("User update failed");
            }

            return Ok();
        }
    }
}