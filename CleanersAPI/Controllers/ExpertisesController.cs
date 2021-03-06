﻿using System.Collections.Generic;
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
    public class ExpertisesController : Controller
    {
        private readonly IExpertiseService _expertiseService;
        private readonly IAuthService _authService;
        private readonly IProfessionalsService _professionalsService;

        public ExpertisesController(IExpertiseService expertiseService, IAuthService authService,
            IProfessionalsService professionalsService)
        {
            _expertiseService = expertiseService;
            _authService = authService;
            _professionalsService = professionalsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expertise>>> GetAll([FromQuery] string serviceId)
        {
            return string.IsNullOrEmpty(serviceId)
                ? Ok(await _expertiseService.GetAll())
                : Ok(await _expertiseService.GetExpertisesByServiceId(int.Parse(serviceId)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expertise>> GetExpertiseById([FromRoute] int id)
        {
            return Ok(await _expertiseService.GetOneById(id));
        }

        [HttpPost("available")]
        public ActionResult<IEnumerable<Expertise>> GetAvailableExpertises(
            [FromBody] AvailabilityFinder availabilityFinder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(_expertiseService.GetAvailableExpertises(availabilityFinder));
        }

        [Authorize(Roles = "Professional")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpertise([FromRoute] int id, [FromBody] Expertise expertise)
        {
            if (!ModelState.IsValid || !id.Equals(expertise.Id))
            {
                return BadRequest(ModelState);
            }

            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _authService.GetUserById(loggedInUserId);

            var profession = await _professionalsService.GetProfessionalByUserId(userFromRepo.Id);
            if (!profession.Id.Equals(expertise.ProfessionalId))
            {
                return StatusCode(403, "You are not allowed to make this modification");
            }

            await _expertiseService.Update(expertise);
            return Ok();
        }
    }
}