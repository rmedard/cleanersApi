using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanersAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public decimal TotalCost { get; set; }
        
        public Status Status { get; set; }
        
        public Expertise Expertise { get; set; }
        
        public Customer Customer { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "Confirmed")]
        Confirmed,
        
        [EnumMember(Value = "Rejected")]
        Rejected,
        
        [EnumMember(Value = "Done")]
        Done
    }
}