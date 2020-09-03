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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservation = await _reservationsService.GetOneById(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
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
            
            if (userFromRepo?.Customer == null || userFromRepo.Customer.Id != reservationForCreate.CustomerId)
            {
                return Unauthorized("You don't have permission to create reservation");
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

            var reservation = _mapper.Map<Reservation>(reservationForCreate);
            var expertise = _expertiseService.FindExpertise(
                reservationForCreate.ExpertiseForServiceCreate.ProfessionalId,
                reservationForCreate.ExpertiseForServiceCreate.ServiceId).Result;
            if (expertise == null)
            {
                return BadRequest("Expertise not found");
            }

            reservation.Expertise = expertise;
            reservation.Customer = userFromRepo.Customer;
            reservation.Status = Status.Confirmed;
            reservation.TotalCost = expertise.HourlyRate * reservationForCreate.Duration;
            var newReservation = _reservationsService.Create(reservation);
            if (newReservation == null)
            {
                return BadRequest("Reservation creation failed");
            }

            return CreatedAtAction("GetReservation", new {id = newReservation.Id}, newReservation);
        }

        [HttpPost("search")]
        public ActionResult<IEnumerable<Reservation>>  GetReservations([FromBody] ReservationSearchCriteriaDto reservationSearchCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var searchCriteria = new ReservationSearchCriteria();
            if (reservationSearchCriteriaDto.CustomerId != 0)
            {
                if (!_customersService.DoesExist(reservationSearchCriteriaDto.CustomerId))
                {
                    return NotFound($"Customer with id {reservationSearchCriteriaDto.CustomerId} not found");
                }
                searchCriteria.Build(_customersService.GetOneById(reservationSearchCriteriaDto.CustomerId).Result);
            }

            if (reservationSearchCriteriaDto.ProfessionalId != 0)
            {
                if (!_professionalsService.DoesExist(reservationSearchCriteriaDto.ProfessionalId))
                {
                    return NotFound($"Professional with id {reservationSearchCriteriaDto.ProfessionalId} not found");
                }
                searchCriteria.Build(_professionalsService.GetOneById(reservationSearchCriteriaDto.ProfessionalId).Result);
            }

            if (!string.IsNullOrEmpty(reservationSearchCriteriaDto.Status))
            {
                if (!Enum.TryParse(reservationSearchCriteriaDto.Status, out Status reservationStatus))
                {
                    return BadRequest("Invalid status");
                }
                searchCriteria.Build(reservationStatus);
            }

            if (!string.IsNullOrEmpty(reservationSearchCriteriaDto.Date))
            {
                if (!DateTime.TryParse(reservationSearchCriteriaDto.Date, out var dateTime))
                {
                    return BadRequest("Invalid date");
                }
                searchCriteria.Build(dateTime);
            }

            if (reservationSearchCriteriaDto.HasBill != null)
            {
                searchCriteria.Build(reservationSearchCriteriaDto.HasBill.Value);
            }

            return new ActionResult<IEnumerable<Reservation>>(_reservationsService.Search(searchCriteria).Result);
        }
    }
}