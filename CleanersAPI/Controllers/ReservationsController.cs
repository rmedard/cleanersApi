using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CleanersAPI.Helpers;
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
        private readonly ICustomersService _customersService;
        private readonly IExpertiseService _expertiseService;
        private readonly IReservationsService _reservationsService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public ReservationsController(IProfessionalsService professionalsService, ICustomersService customersService, IExpertiseService expertiseService,
            IReservationsService reservationsService, IAuthService authService, IMapper mapper)
        {
            _professionalsService = professionalsService;
            _customersService = customersService;
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
                return BadRequest("Reservation creation failed");
            }

            return Ok(newService);
        }

        [HttpGet]
        public IActionResult GetReservations([FromQuery]int customerId = 0, [FromQuery]int professionalId = 0, [FromQuery]string status = "", [FromQuery]string date = "")
        {
            var searchCriteria = new ReservationSearchCriteria();
            if (customerId != 0)
            {
                if (!_customersService.DoesExist(customerId))
                {
                    return NotFound($"Customer with id {customerId} not found");
                }
                searchCriteria.Build(_customersService.GetOneById(customerId).Result);
            }

            if (professionalId != 0)
            {
                if (!_professionalsService.DoesExist(professionalId))
                {
                    return NotFound($"Professional with id {professionalId} not found");
                }
                searchCriteria.Build(_professionalsService.GetOneById(professionalId).Result);
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (!Enum.TryParse(status, out Status reservationStatus))
                {
                    return BadRequest("Invalid status");
                }
                searchCriteria.Build(reservationStatus);
            }

            if (!string.IsNullOrEmpty(date))
            {
                if (!DateTime.TryParse(date, out var dateTime))
                {
                    return BadRequest("Invalid date");
                }
                searchCriteria.Build(dateTime);
            }

            return Ok(_reservationsService.Search(searchCriteria));
        }
    }
}