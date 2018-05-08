namespace CleanersAPI.Models
{
    public class RoleUser
    {
        public int roleId { get; set; }
        public int userId { get; set; }
        public Role role { get; set; }
        public User user { get; set; }
    }
}