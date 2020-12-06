using System;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.User;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICustomersService _customersService;
        private readonly IProfessionalsService _professionalsService;

        public AuthController(IAuthService authService, ICustomersService customersService,
            IProfessionalsService professionalsService)
        {
            _authService = authService;
            _customersService = customersService;
            _professionalsService = professionalsService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _authService.UserExists(userForLoginDto.Username))
            {
                return BadRequest("User does not exist. Please register!");
            }
            
            var userFromRepo = await _authService.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
            {
                return StatusCode(401, "Invalid credentials");
            }

            if (!userFromRepo.IsActive)
            {
                return StatusCode(403, "Your account has been suspended");
            }

            var token = _authService.GenerateLoginToken(userFromRepo);
            var userAccount = new UserForDisplayDto {User = userFromRepo};

            foreach (var roleUser in userFromRepo.Roles)
            {
                switch (roleUser.Role.RoleName)
                {
                    case RoleName.Customer:
                    {
                        var customer = await _customersService.GetCustomerByUserId(userFromRepo.Id);
                        userAccount.CustomerId = customer.Id;
                        break;
                    }
                    case RoleName.Professional:
                    {
                        var professional = await _professionalsService.GetProfessionalByUserId(userFromRepo.Id);
                        userAccount.ProfessionalId = professional.Id;
                        break;
                    }
                    case RoleName.Admin:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return Ok(new {token, userAccount});
        }
    }
}