using System;
using CleanersAPI.Models;

namespace CleanersAPI.Helpers
{
    public class ReservationSearchCriteria
    {
        public Professional Professional { get; private set; }
        public Customer Customer { get; private set; }
        public Status? Status { get; private set; }
        public DateTime? DateTime { get; private set; }
        
        public bool? HasBill {get; private set;}

        public ReservationSearchCriteria()
        {
            Professional = null;
            Customer = null;
            Status = null;
            DateTime = null;
        }

        public ReservationSearchCriteria Build(Professional professional)
        {
            Professional = professional;
            return this;
        }
        
        public ReservationSearchCriteria Build(Customer customer)
        {
            Customer = customer;
            return this;
        }
        
        public ReservationSearchCriteria Build(DateTime dateTime)    
        {
            DateTime = dateTime;
            return this;
        }
        
        public ReservationSearchCriteria Build(Status status)
        {
            Status = status;
            return this;
        }

        public ReservationSearchCriteria Build(bool hasBill)
        {
            HasBill = hasBill;
            return this;
        }
    }
}