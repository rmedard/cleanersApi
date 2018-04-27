using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProfessionsController : Controller
    {
        private readonly CleanersApiContext _context;

        public ProfessionsController(CleanersApiContext context)
        {
            _context = context;
        }

        // GET: api/Professions
        [HttpGet]
        public IEnumerable<Profession> GetProfession()
        {
            return _context.Professions;
        }

        // GET: api/Professions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfession([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profession = await _context.Professions.SingleOrDefaultAsync(m => m.Id == id);

            if (profession == null)
            {
                return NotFound();
            }

            return Ok(profession);
        }

        // PUT: api/Professions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfession([FromRoute] int id, [FromBody] Profession profession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profession.Id)
            {
                return BadRequest();
            }

            _context.Entry(profession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfessionExists(id))
                {
                    return NotFound();
                }
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

            _context.Professions.Add(profession);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfession", new { id = profession.Id }, profession);
        }

        // DELETE: api/Professions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfession([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profession = await _context.Professions.SingleOrDefaultAsync(m => m.Id == id);
            if (profession == null)
            {
                return NotFound();
            }

            _context.Professions.Remove(profession);
            await _context.SaveChangesAsync();

            return Ok(profession);
        }

        private bool ProfessionExists(int id)
        {
            return _context.Professions.Any(e => e.Id == id);
        }
    }
}