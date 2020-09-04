using CleanersAPI.Validators;

namespace CleanersAPI.Models.Dtos.Service
{
    public class ReservationSearchCriteriaDto
    {
        [ReservationSearchCustomerId(ErrorMessage = "ddd")]
        public int CustomerId { get; set; }
        public int ProfessionalId { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        
        public bool? HasBill { get; set; }
    }
}