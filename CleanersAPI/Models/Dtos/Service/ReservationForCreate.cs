using System;
using System.ComponentModel.DataAnnotations;

namespace CleanersAPI.Models.Dtos.Service
{
    public class ReservationForCreate
    {
        [Required] public int CustomerId { get; set; }
        [Required] public ExpertiseForServiceCreate ExpertiseForServiceCreate { get; set; }
        
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [Required] public DateTime StartTime { get; set; }
        [Required] public int Duration { get; set; }
        
        public Recurrence Recurrence { get; set; }
    }
}