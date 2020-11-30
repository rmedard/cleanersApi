using System.Collections.Generic;
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

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return Ok(await _usersService.GetAll());
        }
        
        [HttpPatch("{id:int}")]
        public IActionResult UpdateUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid || !id.Equals(user.Id))
            {
                return BadRequest(ModelState);
            }
            
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!loggedInUserId.Equals(user.Id))
            {
                return StatusCode(403, "You do not have permission to update this user");
            }

            var updated = _usersService.Update(user);
            if (!updated.Result)
            {
                return BadRequest("User update failed");
            }

            return Ok();
        }
    }
}