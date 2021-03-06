﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BillingController : Controller
    {

        private readonly ICustomersService _customersService;
        private readonly IProfessionalsService _professionalsService;
        private readonly IBillingsService _billingsService;
        private readonly IAuthService _authService;

        public BillingController(ICustomersService customersService, 
            IProfessionalsService professionalsService, 
            IBillingsService billingsService, IAuthService authService)
        {
            _customersService = customersService;
            _professionalsService = professionalsService;
            _billingsService = billingsService;
            _authService = authService;
        }

        [Authorize(Roles = "Admin,Customer")]
        [HttpGet]
        public async Task<ActionResult<Billing>> GetBills([FromQuery] string customerId)
        {
            if (!string.IsNullOrEmpty(customerId))
            {
                if (!_customersService.DoesExist(int.Parse(customerId)))
                {
                    return NotFound($"Customer with id {customerId} not found");
                }

                return Ok(await _billingsService.GetBillings(int.Parse(customerId)));
            }
            return Ok(await _billingsService.GetBillings());
        }
        
        [Authorize(Roles = "Admin,Professional")]
        [HttpGet("{customerId}/createBill")]
        public async Task<ActionResult<Billing>> CreateBilling([FromRoute] int customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (!_customersService.DoesExist(customerId))
            {
                return NotFound($"Customer with id {customerId} not found");
            }
            
            var reservationSearchCriteria = new ReservationSearchCriteria()
                .Build(_customersService.GetOneById(customerId).Result)
                .Build(Status.Confirmed)
                .Build(false);
            
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _authService.GetUserById(loggedInUserId);
            if (userFromRepo.Roles.Any(r => r.Role.RoleName.Equals(RoleName.Professional)))
            {
                var professional = _professionalsService.GetProfessionalByUserId(loggedInUserId).Result;
                reservationSearchCriteria = reservationSearchCriteria.Build(professional);
            }
            
            var billing = await _billingsService.Create(reservationSearchCriteria);
            if (billing == null)
            {
                return NotFound("No reservations to bill available");
            }
            
            return Ok(billing);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("createBill")]
        public async Task<IActionResult> CreateAllBillings()
        {
             var customerIds = _customersService.GetAvailableBillableCustomers().Result.Select(c => c.Id).ToList();
             foreach (var customerId in customerIds)
             {
                 await CreateBilling(customerId);
             }
             return Ok();
        }
    }
}