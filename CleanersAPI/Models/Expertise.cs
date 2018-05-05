﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanersAPI.Models
{
    public class Expertise
    {
        [Required]
        public int ProfessionId { get; set; }

        [Required]
        public int ProfessionalId { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public Profession Profession { get; set; }

        public Professional Professional { get; set; }

    }
}