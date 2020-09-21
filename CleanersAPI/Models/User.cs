
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace CleanersAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        
        [JsonIgnore]
        public IList<RoleUser> Roles { get; }

        public User()
        {
            Roles = new Collection<RoleUser>();
        }
        
        // public Customer Customer { get; set; }
        //
        // public Professional Professional { get; set; }
    }
}