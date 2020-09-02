using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CleanersAPI.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required] public string Title { get; set; }
        
        public string Description { get; set; }

        [Required] public Category Category { get; set; }
    }

    public enum Category
    {
        [EnumMember(Value = "Exterieur")]
        Exterieur,
        
        [EnumMember(Value = "Interieur")]
        Interieur
    }
}