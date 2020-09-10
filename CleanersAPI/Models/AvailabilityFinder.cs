using System;
using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models
{
    public class AvailabilityFinder
    {
        [Required]
        public int ServiceId { get; set; }
        
        [Required]
        public DateTime DateTime { get; set; }
        
        [Required]
        public int Duration { get; set; }
    }
}