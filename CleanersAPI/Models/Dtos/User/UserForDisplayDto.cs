namespace CleanersAPI.Models.Dtos.User
{
    public class UserForDisplayDto
    {
        public Models.User User { get; set; }

        public int CustomerId { get; set; }
        
        public int ProfessionalId  { get; set; }
    }
}