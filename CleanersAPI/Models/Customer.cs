﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanersAPI.Models
{
    public class Customer : Person
    {
        public int Id { get; set; }
        
        public string RegNumber { get; set; }
        
        public User User { get; set; }
    }
}