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
    public class ExpertisesController : Controller
    {
        private readonly IExpertiseService _expertiseService;

        public ExpertisesController(IExpertiseService expertiseService)
        {
            _expertiseService = expertiseService;
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
    }
}