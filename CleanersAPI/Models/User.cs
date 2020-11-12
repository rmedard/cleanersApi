
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CleanersAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        
        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        public int AddressId { get; set; }

        public string Picture { get; set; }

        [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; }

        [Required]
        public Address Address { get; set; }
        
        [JsonIgnore]
        public IList<RoleUser> Roles { get; }

        public User()
        {
            Roles = new Collection<RoleUser>();
        }
    }
}