using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : Controller
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService professionsService)
        {
            _servicesService = professionsService;
        }

        [HttpGet]
        public Task<IEnumerable<Service>> GetServices()
        {
            return _servicesService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetService([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profession = await _servicesService.GetOneById(id);

            if (profession == null)
            {
                return NotFound();
            }

            return Ok(profession);
        }

        [HttpPut("{id}")]
        public IActionResult PutService([FromRoute] int id, [FromBody] Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.Id)
            {
                return BadRequest();
            }

            var updated = _servicesService.Update(service);

            if (!updated.Result)
            {
                return BadRequest("Update failed");
            }
            
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProfession = await _servicesService.Create(service); 

            return CreatedAtAction("GetServices", new { id = newProfession.Id }, newProfession);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteService([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_servicesService.DoesExist(id))
            {
                return NotFound("Profession not found");
            }

            var deleted = _servicesService.Delete(id);
            if (!deleted.Result)
            {
                return BadRequest("Deletion failed");
            }
            return Ok();
        }

    }
}