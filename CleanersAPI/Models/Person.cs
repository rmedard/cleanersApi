using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models
{
    public class Person
    {
        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public int AddressId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public Address Address { get; set; }
    }
}