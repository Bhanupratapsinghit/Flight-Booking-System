using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.DTOS
{
    public class CheckInDto
    {
        public int CheckInId { get; set; }
        public int BookingPassengerId { get; set; }
        public DateTime? CheckInTime { get; set; }
        public bool HasCheckedIn { get; set; }
    }

}