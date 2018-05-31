using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.Service;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly IProfessionalsService _professionalsService;
        private readonly IExpertiseService _expertiseService;
        private readonly IServicesService _servicesService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public ServicesController(IProfessionalsService professionalsService, IExpertiseService expertiseService,
            IServicesService servicesService, IAuthService authService, IMapper mapper)
        {
            _professionalsService = professionalsService;
            _expertiseService = expertiseService;
            _servicesService = servicesService;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] ServiceForCreate serviceForCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = _authService.GetUserById(loggedInUserId).Result;
            if (userFromRepo.Customer == null)
            {
                return NotFound("No customer found");
            }

            if (userFromRepo.Customer.Id != serviceForCreate.CustomerId)
            {
                return Unauthorized();
            }
            
            var isFree = _professionalsService.IsFree(serviceForCreate.ExpertiseForServiceCreate.ProfessionalId,
                serviceForCreate.StartTime, serviceForCreate.Duration);
            if (!isFree)
            {
                return BadRequest("Professional not available");
            }

            var service = _mapper.Map<Service>(serviceForCreate);
            var expertise = _expertiseService.FindExpertise(
                serviceForCreate.ExpertiseForServiceCreate.ProfessionalId,
                serviceForCreate.ExpertiseForServiceCreate.ProfessionId).Result;
            if (expertise == null)
            {
                return BadRequest("Expertise not found");
            }

            service.Customer = userFromRepo.Customer;
            service.Status = Status.Initiated;
            service.TotalCost = expertise.UnitPrice * serviceForCreate.Duration;
            var newService = _servicesService.Create(service);
            if (newService == null)
            {
                return BadRequest("Order creation failed");
            }

            return Ok(newService);
        }
    }
}