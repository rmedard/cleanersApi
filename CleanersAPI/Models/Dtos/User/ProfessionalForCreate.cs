using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models.Dtos.User
{
    public class ProfessionalForCreate
    {
        [Required]
        public Professional Professional { get; set; }

        [Required]
        public string Password { get; set; }
    }
}