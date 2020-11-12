using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        
        public Order? Order { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Order
    {
        [EnumMember(Value = "asc")]
        Ascendant,
        [EnumMember(Value = "desc")]
        Descendant
    }
}