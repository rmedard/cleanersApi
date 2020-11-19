using System.Collections.Generic;

namespace CleanersAPI.Models
{
    public class Professional
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public bool IsActive { get; set; }
        public ICollection<Expertise> Expertises { get; } = new List<Expertise>();
    }
}