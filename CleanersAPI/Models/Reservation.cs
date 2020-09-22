using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanersAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public decimal TotalCost { get; set; }
        
        [Required]
        public Status Status { get; set; }
        
        public Expertise Expertise { get; set; }
        
        public Customer Customer { get; set; }
        
        public int? BillingId { get; set; }
        
        public Billing Billing { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "Confirmed")]
        Confirmed,
        [EnumMember(Value = "Rejected")]
        Rejected
    }
}