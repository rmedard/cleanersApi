using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models
{
    public class Expertise
    {
        public int Id { get; set; }
        
        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int ProfessionalId { get; set; }
        
        public decimal HourlyRate { get; set; }
        
        public bool IsActive { get; set; }
        
        public Service Service { get; set; }
        
        public Professional Professional { get; set; }

    }
}
