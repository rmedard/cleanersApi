using System.ComponentModel.DataAnnotations;
using CleanersAPI.Models;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MailsController : Controller
    {
        private readonly IEmailsService _emailsService;

        public MailsController(IEmailsService emailsService)
        {
            _emailsService = emailsService;
        }

        [HttpPost]
        public IActionResult SendEmail([FromBody] Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(email.From) || !emailValidator.IsValid(email.To))
            {
                return BadRequest("Invalid email address...");
            }

            var newEmail = _emailsService.Create(email);
            if (newEmail == null || !newEmail.Result.Sent)
            {
                return NotFound("Unable to send email...");
            }

            return Ok(newEmail);
        }
    }
}