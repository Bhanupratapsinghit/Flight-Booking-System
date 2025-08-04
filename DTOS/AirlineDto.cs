using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FlightProject.DTOs
{
    public class AirlineDto
    {
        public int AirlineId { get; set; }

        [Required]
        public string AirlineName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string OperatingRegion { get; set; }
    }

    public class AddAirlineDto
    {
        public int AirlineId { get; set; }
        [Required]
        public string AirlineName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string OperatingRegion { get; set; }
    }
}
