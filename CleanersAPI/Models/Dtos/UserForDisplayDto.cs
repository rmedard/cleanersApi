using System.Collections.Generic;

namespace CleanersAPI.Models.Dtos
{
    public class UserForDisplayDto
    {
        public int Id { get; set; }

        public string Username { get; set; }
        
        public ICollection<string> Roles { get; set; }
        
        public int professionalId { get; set; }
        
        public int customerId { get; set; }
    }
}