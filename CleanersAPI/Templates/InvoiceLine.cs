using System;

namespace CleanersAPI.Templates
{
    public class InvoiceLine
    {
        public string Service { get; set; }
        
        public DateTime DateTime { get; set; }
        public int NumberOfHours { get; set; }
        
        public decimal HourlyRate { get; set; }
        
        public decimal TotalCost { get; set; }
    }
}