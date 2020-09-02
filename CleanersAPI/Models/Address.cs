using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string PlotNumber { get; set; }

        [Required]
        public string StreetName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int PostalCode { get; set; }
        
        public string GeoLocation { get; set; }
    }
}
