using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CleanersAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        public bool IsActive { get; set; }
        
        public IEnumerable<Reservation> Reservations { get; } = new List<Reservation>();
    }
}
