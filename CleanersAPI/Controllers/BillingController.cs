using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BillingController : Controller
    {

        private readonly ICustomersService _customersService;

        private readonly IBillingsService _billingsService;

        public BillingController(ICustomersService customersService, IBillingsService billingsService)
        {
            _customersService = customersService;
            _billingsService = billingsService;
        }

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
            
            var reservationSearchCriteria = new ReservationSearchCriteria();
            reservationSearchCriteria.Build(_customersService.GetOneById(customerId).Result);
            reservationSearchCriteria.Build(false);
            return _billingsService.Create(reservationSearchCriteria).Result;
        }
    }
}