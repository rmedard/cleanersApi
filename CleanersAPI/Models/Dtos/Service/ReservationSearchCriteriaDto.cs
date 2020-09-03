namespace CleanersAPI.Models.Dtos.Service
{
    public class ReservationSearchCriteriaDto
    {
        public int CustomerId { get; set; }
        public int ProfessionalId { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
    }
}