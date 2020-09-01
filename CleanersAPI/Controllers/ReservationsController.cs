using System.Security.Claims;
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
    [ApiController]
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IProfessionalsService _professionalsService;
        private readonly IExpertiseService _expertiseService;
        private readonly IReservationsService _reservationsService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public ReservationsController(IProfessionalsService professionalsService, IExpertiseService expertiseService,
            IReservationsService reservationsService, IAuthService authService, IMapper mapper)
        {
            _professionalsService = professionalsService;
            _expertiseService = expertiseService;
            _reservationsService = reservationsService;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody] ReservationForCreate reservationForCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = _authService.GetUserById(loggedInUserId).Result;
            
            if (userFromRepo == null ||  userFromRepo.Customer.Id != reservationForCreate.CustomerId)
            {
                return Unauthorized();
            }
            
            if (userFromRepo.Customer == null)
            {
                return NotFound("No customer found");
            }
            
            var isFree = _professionalsService.IsFree(reservationForCreate.ExpertiseForServiceCreate.ProfessionalId,
                reservationForCreate.StartTime, reservationForCreate.Duration);
            if (!isFree)
            {
                return BadRequest("Professional not available");
            }

            var service = _mapper.Map<Reservation>(reservationForCreate);
            var expertise = _expertiseService.FindExpertise(
                reservationForCreate.ExpertiseForServiceCreate.ProfessionalId,
                reservationForCreate.ExpertiseForServiceCreate.ProfessionId).Result;
            if (expertise == null)
            {
                return BadRequest("Expertise not found");
            }

            service.Expertise = expertise;
            service.Customer = userFromRepo.Customer;
            service.Status = Status.Confirmed;
            service.TotalCost = expertise.HourlyRate * reservationForCreate.Duration;
            var newService = _reservationsService.Create(service);
            if (newService == null)
            {
                return BadRequest("Order creation failed");
            }

            return Ok(newService);
        }
    }
}