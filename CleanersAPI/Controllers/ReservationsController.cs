﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.Service;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IProfessionalsService _professionalsService;
        private readonly ICustomersService _customersService;
        private readonly IExpertiseService _expertiseService;
        private readonly IReservationsService _reservationsService;
        private readonly IEmailsService _emailsService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public ReservationsController(IProfessionalsService professionalsService, ICustomersService customersService,
            IExpertiseService expertiseService, IEmailsService emailsService,
            IReservationsService reservationsService, IAuthService authService, IMapper mapper)
        {
            _professionalsService = professionalsService;
            _customersService = customersService;
            _expertiseService = expertiseService;
            _reservationsService = reservationsService;
            _emailsService = emailsService;
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

        /// <summary>
        /// Searches reservations.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Reservations
        ///     {
        ///         "customerId": 1,
        ///         "expertiseForServiceCreate": {
        ///             "professionalId": 1,
        ///             "serviceId": 1
        ///         },
        ///         "startTime": "2020-09-03 10:00:00",
        ///         "duration": 2
        ///     }
        /// 
        /// </remarks>
        /// <param name="reservationForCreate"></param>
        /// <returns>A the newly created reservation</returns>
        /// <response code="201">Returns the newly created reservation</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="400">If reservation object is invalid or either 'professional' or 'Expertise' does not exist</response>
        /// <response code="403">If logged-in user is neither 'admin' nor 'customer'</response>
        /// <response code="404">If 'customer' does not exist</response>
        // [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin,Customer")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Reservation>> CreateReservation([FromBody] ReservationForCreate reservationForCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = _authService.GetUserById(loggedInUserId).Result;
            var customer = await _customersService.GetCustomerByUserId(userFromRepo.Id);
            if (customer == null || !(customer.Id == reservationForCreate.CustomerId &&
                                      userFromRepo.Id.Equals(customer.UserId)))
            {
                return StatusCode(403, "You do not have permission to create this reservation");
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
            reservation.Customer = customer;
            reservation.Status = Status.Confirmed;
            reservation.TotalCost = expertise.HourlyRate * reservationForCreate.Duration;
            var newReservation = await _reservationsService.Create(reservation);
            if (newReservation == null)
            {
                return BadRequest("Reservation creation failed");
            }

            await _emailsService.notifyUsersOnReservationCreation(newReservation);

            return CreatedAtAction("GetReservation", new {id = newReservation.Id}, newReservation);
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations(
            [FromBody] ReservationSearchCriteriaDto reservationSearchCriteriaDto)
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

                searchCriteria.Build(_professionalsService.GetOneById(reservationSearchCriteriaDto.ProfessionalId)
                    .Result);
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

            var reservations = await _reservationsService.Search(searchCriteria);
            return new ActionResult<IEnumerable<Reservation>>(reservations);
        }

        [HttpGet("{id}/cancel")]
        public async Task<IActionResult> CancelReservation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservation = await _reservationsService.GetOneById(id);
            if (reservation == null)
            {
                return NotFound("Reservation not found");
            }

            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = _authService.GetUserById(loggedInUserId).Result;
            var userRole = userFromRepo.Roles[0].Role.RoleName;

            switch (userRole)
            {
                case RoleName.Customer:
                {
                    var customer = await _customersService.GetCustomerByUserId(reservation.CustomerId);
                    if (customer != null &&
                        !(customer.Id == reservation.CustomerId && userFromRepo.Id.Equals(customer.UserId)))
                    {
                        return StatusCode(403, "You don't have permission to update this reservation");
                    }

                    break;
                }
                case RoleName.Professional:
                {
                    var professional = await _professionalsService.GetProfessionalByUserId(loggedInUserId);
                    if (professional != null && !professional.Id.Equals(reservation.Expertise.ProfessionalId))
                    {
                        return StatusCode(403, "You don't have permission to update this reservation");
                    }

                    break;
                }
                case RoleName.Admin:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (reservation.BillingId != null)
            {
                return BadRequest("You cannot cancel billed reservation");
            }

            if (reservation.StartTime.CompareTo(DateTime.Now) <= 0)
            {
                return BadRequest("You cannot cancel reservation in the past");
            }

            if (!reservation.Status.Equals(Status.Confirmed))
            {
                return NoContent();
            }

            reservation.Status = Status.Rejected;
            await _reservationsService.Update(reservation);
            return Ok("Reservation cancelled successfully");
        }
    }
}