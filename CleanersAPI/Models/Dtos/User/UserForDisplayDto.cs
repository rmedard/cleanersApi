using System.Collections.Generic;

namespace CleanersAPI.Models.Dtos.User
{
    public class UserForDisplayDto
    {
        public int Id { get; set; }

        public string Username { get; set; }
        
        public ICollection<string> Roles { get; set; }

        public Person Person { get; set; }
    }
}