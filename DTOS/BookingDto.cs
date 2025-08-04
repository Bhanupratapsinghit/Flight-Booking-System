using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.DTOS
{
   public class BookingDto
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public string UserId { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancellationDate { get; set; }
    }
}