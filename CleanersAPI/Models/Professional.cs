﻿namespace CleanersAPI.Models
{
    public class Professional : Person
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        
        public User User { get; set; }
    }
}