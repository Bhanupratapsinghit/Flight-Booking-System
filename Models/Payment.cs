using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }

        // Navigation property
        public Booking Booking { get; set; }
    }
}