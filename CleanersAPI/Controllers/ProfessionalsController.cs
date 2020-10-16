using System.Collections.Generic;
using System.Linq;
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
    public class ProfessionalsController : Controller
    {
        private readonly IProfessionalsService _professionalsService;
        private readonly IServicesService _servicesService;
        private readonly IAuthService _authService;

        public ProfessionalsController(IProfessionalsService professionalsService,
            IServicesService servicesService,
            IAuthService authService)
        {
            _professionalsService = professionalsService;
            _servicesService = servicesService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professional>>> GetProfessionals()
        {
            return Ok(await _professionalsService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfessional([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var professional = await _professionalsService.GetOneById(id);

            if (professional == null)
            {
                return NotFound();
            }

            return Ok(professional);
        }

        [HttpGet("userId/{id}")]
        public async Task<IActionResult> GetProfessionalByUserId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _professionalsService.GetProfessionalByUserId(id));
        }
        
        [HttpPost("available")]
        public async Task<ActionResult<IEnumerable<Professional>>> GetAvailableProfessionals([FromBody] AvailabilityFinder availabilityFinder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _professionalsService.GetAvailableProfessionals(availabilityFinder));
        }

        [HttpGet("{id}/expertises")]
        public ActionResult<IEnumerable<Expertise>> GetProfessionalExpertises([FromRoute] int id)
        {
            return !ProfessionalExists(id)
                ? NotFound("No professional found")
                : new ActionResult<IEnumerable<Expertise>>(_professionalsService.GetExpertises(id).Result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProfessional([FromRoute] int id, [FromBody] Professional professional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != professional.Id)
            {
                return BadRequest();
            }

            var updated = _professionalsService.Update(professional);
            if (!updated.Result)
            {
                return BadRequest("Update failed");
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfessional([FromBody] ProfessionalForCreate professionalForCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_authService.UserExists(professionalForCreate.Professional.Email).Result)
            {
                return BadRequest("Professional already exists. Please login!!");
            }

            professionalForCreate.Professional.User = _authService.GenerateUserAccount(professionalForCreate.Professional, professionalForCreate.Password);
            var newProfessional = await _professionalsService.Create(professionalForCreate.Professional);

            return CreatedAtAction("GetProfessional", new {id = newProfessional.Id}, newProfessional);
        }

        [HttpPost("{id}/expertises")]
        public IActionResult AddExpertise([FromRoute] int id, [FromBody] Expertise expertise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expertise.ProfessionalId)
            {
                return BadRequest();
            }

            if (!_professionalsService.DoesExist(expertise.ProfessionalId))
            {
                return NotFound("Professional not found");
            }

            if (!_servicesService.DoesExist(expertise.ServiceId))
            {
                return NotFound("Service not found");
            }

            var professional = _professionalsService.GetOneById(expertise.ProfessionalId);
            if (professional.Result.Expertises.AsQueryable().Any(e => e.ServiceId.Equals(expertise.ServiceId)))
            {
                return BadRequest("Professional does already propose this service");
            }
            
            _professionalsService.GrantExpertise(expertise);
            return Ok();
        }

        [HttpPut("{id}/expertises")]
        public IActionResult UpdateExpertise([FromRoute] int id, [FromBody] Expertise expertise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expertise.ProfessionalId)
            {
                return BadRequest();
            }

            _professionalsService.UpdateExpertise(expertise);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProfessional([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!ProfessionExists(id))
            {
                return NotFound();
            }

            var deleted = _professionalsService.Delete(id);
            if (deleted.Result)
            {
                return Ok();
            }

            return BadRequest("Delete failed");
        }

        private bool ProfessionalExists(int id)
        {
            return _professionalsService.DoesExist(id);
        }

        private bool ProfessionExists(int id)
        {
            return _servicesService.DoesExist(id);
        }
    }
}