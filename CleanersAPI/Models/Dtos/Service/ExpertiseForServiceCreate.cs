using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models.Dtos.Service
{
    public class ExpertiseForServiceCreate
    {
        [Required]
        public int ProfessionalId { get; set; }
        
        [Required]
        public int ServiceId { get; set; }
        
    }
}