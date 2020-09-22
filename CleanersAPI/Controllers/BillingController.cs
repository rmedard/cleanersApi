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

        [Authorize(Roles = "Admin")]
        [HttpGet("{customerId}/createBill")]
        public ActionResult<Billing> CreateBilling([FromRoute] int customerId)
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
            var billing = _billingsService.Create(reservationSearchCriteria);
            if (billing == null)
            {
                return NotFound("No reservations to bill available");
            }
            return billing.Result;
        }
        
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Professional")]
        [HttpGet("{customerId}/{professionalId}/createBill")]
        public ActionResult<Billing> CreateBilling([FromRoute] int customerId, [FromRoute] int professionalId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (!_customersService.DoesExist(customerId))
            {
                return NotFound($"Customer with id {customerId} not found");
            }

            if (!_professionalsService.DoesExist(professionalId))
            {
                return NotFound($"Professional with id {professionalId} not found");
            }
            
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = _authService.GetUserById(loggedInUserId).Result;
            var userRole = userFromRepo.Roles[0].Role.RoleName;
            if (userRole.Equals(RoleName.Professional))
            {
                var professional = _professionalsService.GetProfessionalByUserId(loggedInUserId).Result;
                if (!professional.Id.Equals(professionalId))
                {
                    return new ForbidResult("Professional not allowed");
                }
            }
            
            var reservationSearchCriteria = new ReservationSearchCriteria()
                .Build(_customersService.GetOneById(customerId).Result)
                .Build(_professionalsService.GetOneById(professionalId).Result)
                .Build(Status.Confirmed)
                .Build(false);
            var billing = _billingsService.Create(reservationSearchCriteria);
            if (billing == null)
            {
                return NotFound("No reservations to bill available");
            }
            return billing.Result;
        }
    }
}