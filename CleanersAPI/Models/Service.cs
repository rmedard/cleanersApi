using System;

namespace CleanersAPI.Models
{
    public class Service
    {
        public int Id { get; set; }

        public int ExpertiseId { get; set; }
        
        public int CustomerId { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public int Duration { get; set; }

        public decimal TotalCost { get; set; }
        
        public Status Status { get; set; }
        
        public Expertise Expertise { get; set; }
        
        public Customer Customer { get; set; }
    }
    
    public enum Status
    {
        Initiated,
        Accepted,
        Rejected,
        Done
    }
}