﻿using System;

namespace CleanersAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public decimal TotalCost { get; set; }
        
        public Status Status { get; set; }
        
        public Expertise Expertise { get; set; }
        
        public Customer Customer { get; set; }
    }
    
    public enum Status
    {
        Confirmed,
        Rejected,
        Done
    }
}