﻿using System;
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

            switch (userFromRepo.Roles[0].Role.RoleName)
            {
                case RoleName.Admin:
                    
                    break;
                case RoleName.Customer:
                    var customer = await _customersService.GetCustomerByUserId(userFromRepo.Id);
                    user.CustomerId = customer.Id;
                    break;
                case RoleName.Professional:
                    var professional = await _professionalsService.GetProfessionalByUserId(userFromRepo.Id);
                    user.ProfessionalId = professional.Id;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Ok(new {token, user});
        }
    }
}