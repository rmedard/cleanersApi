using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models
{
    public class Recurrence
    {
        public int Id { get; set; }
        
        [Required] public string Label { get; set; }
        
        [Required] public bool IsActive { get; set; }
    }
}