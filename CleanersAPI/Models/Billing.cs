using System;
using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models
{
    public class Billing
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
    }
}