using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.DTOS
{
    public class FlightDto
{
    public int AirlineId { get; set; }
    public string FlightNumber { get; set; }
    
    [Required]
    public DateTime DepartureDateTime { get; set; }  // Ensure this is before ArrivalDateTime
    
    [Required]
    public DateTime ArrivalDateTime { get; set; }

    public string OriginAirportCode { get; set; }
    public string DestinationAirportCode { get; set; }
    public int AvailableSeats { get; set; }

    public decimal Fare { get; set; }
}


}