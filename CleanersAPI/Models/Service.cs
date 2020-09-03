using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanersAPI.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required] public string Title { get; set; }
        
        public string Description { get; set; }
        
        [Required] public Category Category { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Category
    {
        [EnumMember(Value = "Exterieur")]
        Exterieur = 0,
        
        [EnumMember(Value = "Interieur")]
        Interieur = 1
    }
}