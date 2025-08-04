using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.DTOs
{
    public class AirportDto
    {
        public string AirportCode { get; set; }
        public string AirportName { get; set; }
        public string Location { get; set; }
        public string Facilities { get; set; }
    }
}
