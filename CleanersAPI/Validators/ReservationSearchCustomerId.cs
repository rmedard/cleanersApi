using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CleanersAPI.Validators
{
    public class ReservationSearchCustomerId : ValidationAttribute
    {
        // private readonly UserManager<IdentityUser> _userManager;
        //
        // public ReservationSearchCustomerId(UserManager<IdentityUser> userManager)
        // {
        //     _userManager = userManager;
        // }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userManager = validationContext.GetService(typeof(UserManager<IdentityUser>));
            var customerId = Convert.ToInt32(value);
            return new ValidationResult("Inv...");
        }
    }
}