using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FlightProject.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string Role { get; set; }
        
        public ICollection<Passenger> Passengers { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}