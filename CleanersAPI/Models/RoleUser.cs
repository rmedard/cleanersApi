﻿namespace CleanersAPI.Models
{
    public class RoleUser
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
    }
}