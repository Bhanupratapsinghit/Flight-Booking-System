using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.DTOS
{
    public class UpdateBookingDTO
{
    public int FlightId { get; set; }
    public string UserId { get; set; } // Added UserId for updates
    public string PaymentStatus { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancellationDate { get; set; }
}

}