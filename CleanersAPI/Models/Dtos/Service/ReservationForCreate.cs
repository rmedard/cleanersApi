using System;

namespace CleanersAPI.Models.Dtos.Service
{
    public class ReservationForCreate
    {
        public int CustomerId { get; set; }
        public ExpertiseForServiceCreate ExpertiseForServiceCreate { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
    }
}