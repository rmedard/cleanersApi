﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CleanersAPI.Models
{
    public class Professional : Person
    {
        public int Id { get; set; }
        
        public string RegNumber { get; set; }
        
        public User User { get; set; }
        
//        [JsonIgnore]
        public ICollection<Expertise> Expertises { get; } = new List<Expertise>();
    }
}