using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanersAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        
        public RoleName RoleName { get; set; }
        
        [JsonIgnore]
        public ICollection<RoleUser> Users { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoleName
    {
        [EnumMember(Value = "Admin")]
        Admin,
        
        [EnumMember(Value = "Customer")]
        Customer, 
        
        [EnumMember(Value = "Professional")]
        Professional
    }
}