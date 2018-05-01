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
    public class ProfessionsController : Controller
    {
        private readonly IProfessionsService _professionsService;

        public ProfessionsController(IProfessionsService professionsService)
        {
            _professionsService = professionsService;
        }

        // GET: api/Professions
        [HttpGet]
        public Task<IEnumerable<Profession>> GetProfession()
        {
            return _professionsService.GetAll();
        }

        // GET: api/Professions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfession([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profession = await _professionsService.GetOneById(id);

            if (profession == null)
            {
                return NotFound();
            }

            return Ok(profession);
        }

        // PUT: api/Professions/5
        [HttpPut("{id}")]
        public IActionResult PutProfession([FromRoute] int id, [FromBody] Profession profession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profession.Id)
            {
                return BadRequest();
            }

            var updated = _professionsService.Update(profession);

            if (!updated.Result)
            {
                return BadRequest("Update failed");
            }
            
            return NoContent();
        }

        // POST: api/Professions
        [HttpPost]
        public async Task<IActionResult> PostProfession([FromBody] Profession profession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProfession = await _professionsService.Create(profession); 

            return CreatedAtAction("GetProfession", new { id = newProfession.Id }, newProfession);
        }

        // DELETE: api/Professions/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProfession([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_professionsService.DoesExist(id))
            {
                return NotFound("Profession not found");
            }

            var deleted = _professionsService.Delete(id);
            if (!deleted.Result)
            {
                return BadRequest("Deletion failed");
            }
            return Ok();
        }

    }
}