using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : Controller
    {
        private readonly IEmailsService _emailsService;

        public MailsController(IEmailsService emailsService)
        {
            _emailsService = emailsService;
        }

        [HttpGet]
        public Task<IEnumerable<Email>> GetEmails()
        {
            return _emailsService.GetAll();
        }

        [HttpPost]
        public IActionResult SendEmail([FromBody] Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(email.From) 
                || !emailValidator.IsValid(email.To) 
                || !emailValidator.IsValid(email.ReplyTo))
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