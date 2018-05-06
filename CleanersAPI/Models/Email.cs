using System;
using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models
{
    public class Email
    {
        public int Id { get; set; }
        
        [Required]
        public string SenderNames { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string From { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string To { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string ReplyTo { get; set; }
        
        [Required]
        public string Subject { get; set; }
        
        [Required]
        public string Body { get; set; }
        
        public bool Sent { get; set; }
        
        public DateTime Date { get; set; } = DateTime.Now;
    }
}