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
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid from email")]
        public string From { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid to email")]
        public string To { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid replyTo email")]
        public string ReplyTo { get; set; }
        
        [Required(ErrorMessage = "Email subject can't be empty")]
        public string Subject { get; set; }
        
        [Required(ErrorMessage = "Email body can't be empty")]
        public string Body { get; set; }
        
        public string PlainTextBody { get; set; }
        
        public bool Sent { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}