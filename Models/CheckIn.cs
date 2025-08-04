using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightProject.Models
{
    public class CheckIn
    {
        [Key]
        public int CheckInId { get; set; } // Primary Key

        [ForeignKey("BookingPassenger")]
        public int BookingPassengerId { get; set; }

        public DateTime? CheckInTime { get; set; } // Nullable initially

        public bool HasCheckedIn { get; set; } = false; // Default is false

        // Navigation property
        public virtual BookingPassenger BookingPassenger { get; set; }
    }
}
