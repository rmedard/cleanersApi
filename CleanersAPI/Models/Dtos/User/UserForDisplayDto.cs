using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models.Dtos.User
{
    public class UserForDisplayDto
    {
        public int Id { get; set; }

        public string Username { get; set; }
        
        public ICollection<string> Roles { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        public int AddressId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        public string Picture { get; set; }

        [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; }

        public int CustomerId { get; set; }
        
        public int ProfessionalId  { get; set; }
    }
}