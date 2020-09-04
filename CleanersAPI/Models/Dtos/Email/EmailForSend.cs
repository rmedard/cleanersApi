using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models.Dtos.Email
{
    public class EmailForSend
    {
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
    }
}