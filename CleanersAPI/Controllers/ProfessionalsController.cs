using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CleanersAPI.Models;
using CleanersAPI.Services;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProfessionalsController : Controller
    {
        private readonly IProfessionalsService _professionalsService;
        private readonly IProfessionsService _professionsService;

        public ProfessionalsController(IProfessionalsService professionalsService, IProfessionsService professionsService)
        {
            _professionalsService = professionalsService;
            _professionsService = professionsService;
        }

        [HttpGet]
        public Task<IEnumerable<Professional>> GetProfessionals()
        {
            return _professionalsService.GetAll();
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

        [HttpGet("{id}/expertises")]
        public IActionResult GetProfessionalExpertises([FromRoute] int id)
        {
            if (!ProfessionalExists(id))
            {
                return NotFound();
            }
            return Ok(_professionalsService.GetProfessions(id));
        }

        [HttpPut("{id}")]
        public IActionResult PutProfessional([FromRoute] int id, [FromBody] Professional professional)
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

        // POST: api/Professionals
        [HttpPost]
        public async Task<IActionResult> PostProfessional([FromBody] Professional professional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProfessional = await _professionalsService.Create(professional);

            return CreatedAtAction("GetProfessional", new { id = newProfessional.Id }, newProfessional);
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

        // DELETE: api/Professionals/5
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

        [HttpGet("{professionalId}/orders")]
        public async Task<IActionResult> GetOrders([FromRoute] int professionalId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (!ProfessionalExists(professionalId))
            {
                return NotFound("Professional not found");
            }

            return Ok(await _professionalsService.GetOrders(professionalId));
        }
        
        
        private bool ProfessionalExists(int id)
        {
            return _professionalsService.DoesExist(id);
        }

        private bool ProfessionExists(int id)
        {
            return _professionsService.DoesExist(id);
        }
    }
}