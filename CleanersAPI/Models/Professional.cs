using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CleanersAPI.Models
{
    public class Professional
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        
        public Person Person { get; set; }

        [JsonIgnore]
        public ICollection<Expertise> Expertises { get; } = new List<Expertise>();
    }
}