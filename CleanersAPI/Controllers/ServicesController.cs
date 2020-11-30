using System;
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
    public class ServicesController : Controller
    {
        private readonly IServicesService _servicesService;
        private readonly IProfessionalsService _professionalsService;

        public ServicesController(IServicesService professionsService, IProfessionalsService professionalsService)
        {
            _servicesService = professionsService;
            _professionalsService = professionalsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Service>> GetServices([FromQuery] string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return Ok(_servicesService.GetAll().Result);
            }

            if (!Enum.TryParse(category, out Category theCategory))
            {
                return BadRequest("Invalid category name");
            }
            return Ok(_servicesService.GetServicesByCategory(theCategory).Result);
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newService = await _servicesService.Create(service);

            return CreatedAtAction("GetServices", new { id = newService.Id }, newService);
        }
    }
}