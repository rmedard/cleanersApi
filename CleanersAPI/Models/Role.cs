using System.Collections.Generic;

namespace CleanersAPI.Models
{
    public class Role
    {
        public int Id { get; set; }
        public RoleName Name { get; set; }
        
        public ICollection<RoleUser> Users { get; set; }
    }

    public enum RoleName
    {
        ADMIN, USER
    }
}