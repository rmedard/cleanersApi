using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanersAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        
        public RoleName RoleName { get; set; }
        
        public ICollection<RoleUser> Users { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoleName
    {
        Admin, Customer, Professional
    }
}