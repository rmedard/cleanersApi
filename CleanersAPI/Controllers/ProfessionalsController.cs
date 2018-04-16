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

        // GET: api/Professionals
        [HttpGet]
        public IEnumerable<Professional> GetProfessional()
        {
            return _professionalsService.GetAllProfessionals();
        }

        // GET: api/Professionals/5
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

        // PUT: api/Professionals/5
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
            if (updated)
            {
                return NoContent();
            }

            return BadRequest("Update failed");

        }

        // POST: api/Professionals
        [HttpPost]
        public async Task<IActionResult> PostProfessional([FromBody] Professional professional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _professionalsService.Create(professional);

            return CreatedAtAction("GetProfessional", new { id = professional.Id }, professional);
        }

        [HttpPost("{professionalId}/{professionId}")]
        public IActionResult AddExpertise([FromRoute] int professionalId, [FromRoute] int professionId)
        {
            if (!ProfessionalExists(professionalId) || !ProfessionExists(professionId))
            {
                return NotFound();
            }

            _professionalsService.GrantExpertise(professionalId, professionId);
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
            if (deleted)
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
            return _professionsService.DoesExist(id);
        }
    }
}