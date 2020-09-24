using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.Email;
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

        private readonly IMapper _mapper;

        public MailsController(IEmailsService emailsService, IMapper mapper)
        {
            _emailsService = emailsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Email>>> GetEmails()
        {
            return Ok(await _emailsService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailForSend emailForSend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = _mapper.Map<Email>(emailForSend);
            var response = await _emailsService.SendEmail(email);

            if (response.StatusCode.Equals(HttpStatusCode.Accepted))
            {
                email.Sent = true;
            }
            
            var newEmail = await _emailsService.Create(email);
            
            if (!response.StatusCode.Equals(HttpStatusCode.Accepted))
            {
                return BadRequest(response);
            }
            
            return Ok(newEmail);
        }
    }
}