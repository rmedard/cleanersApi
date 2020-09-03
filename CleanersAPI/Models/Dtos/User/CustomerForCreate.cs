using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models.Dtos.User
{
    public class CustomerForCreate
    {
        [Required]
        public Customer Customer { get; set; }

        [Required]
        public string Password { get; set; }
    }
}