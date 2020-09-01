using System;
using CleanersAPI.Models;

namespace CleanersAPI.Helpers
{
    public class ReservationSearchCriteria
    {
        public Professional _professional;
        public Customer _customer;
        public Status _status;
        public DateTime _dateTime;

        public ReservationSearchCriteria build(Professional professional)
        {
            this._professional = professional;
            return this;
        }
        
        public ReservationSearchCriteria build(Customer customer)
        {
            this._customer = customer;
            return this;
        }
        
        public ReservationSearchCriteria build(DateTime dateTime)    
        {
            this._dateTime = dateTime;
            return this;
        }
        
        public ReservationSearchCriteria build(Status status)
        {
            this._status = status;
            return this;
        }
    }
}