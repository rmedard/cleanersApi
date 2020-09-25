using System.Threading.Tasks;
using AutoMapper;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.User;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CleanersAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICustomersService _customersService;
        private readonly IProfessionalsService _professionalsService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, ICustomersService customersService,
            IProfessionalsService professionalsService, IConfiguration configuration, IMapper mapper)
        {
            _authService = authService;
            _customersService = customersService;
            _professionalsService = professionalsService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userFromRepo = await _authService.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var token = _authService.GenerateLoginToken(userFromRepo);
            var user = _mapper.Map<UserForDisplayDto>(userFromRepo);
            user.Person = userFromRepo.Roles[0].Role.RoleName switch
            {
                RoleName.Customer => await _customersService.GetCustomerByUserId(userFromRepo.Id),
                RoleName.Professional => await _professionalsService.GetProfessionalByUserId(userFromRepo.Id),
                RoleName.Admin => new Person
                {
                    Email = user.Username,
                    FirstName = "Admin",
                    LastName = "HouseCleaners",
                    PhoneNumber = "+123456789",
                    IsActive = true,
                    Address = new Address
                    {
                        City = "Brussels",
                        PlotNumber = "123",
                        PostalCode = 3300,
                        StreetName = "Rue de Bruxelles",
                        Id = 0
                    }
                },
                _ => user.Person
            };

            return Ok(new {token, user});
        }
    }
}