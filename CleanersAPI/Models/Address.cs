using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanersAPI.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string Number { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Commune { get; set; }

        [Required]
        public string Zipcode { get; set; }
    }
}
