using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightProject.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("Flight")]
        public int FlightId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        public bool IsCancelled { get; set; } = false; // Cancellation flag (Default: False)
        public DateTime? CancellationDate { get; set; } // Nullable cancellation timestamp

        public decimal TotalFare{get;set;}

        // Navigation properties
        public virtual Flight Flight { get; set; }
        public virtual User User { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<BookingPassenger> BookingPassengers { get; set; }
    }
}
