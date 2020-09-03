using System.Collections.Generic;
using Newtonsoft.Json;

namespace CleanersAPI.Models
{
    public class Customer : Person
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public IEnumerable<Reservation> Reservations { get; } = new List<Reservation>();
    }
}
