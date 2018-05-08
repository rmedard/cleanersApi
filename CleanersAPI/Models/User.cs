
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CleanersAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
        
        public ICollection<RoleUser> Roles { get; }

        public User()
        {
            Roles = new Collection<RoleUser>();
        }
        
        public Professional Professional { get; set; }
        public Customer Customer { get; set; }
    }
}