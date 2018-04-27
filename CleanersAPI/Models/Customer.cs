﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanersAPI.Models
{
    public class Customer : Person
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public User User { get; set; }
    }
}
