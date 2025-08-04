using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightProject.Models
{
    public class Airline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string ContactNumber { get; set; }
        public string OperatingRegion { get; set; }

        // Navigation properties
         [JsonIgnore]
        public ICollection<Flight> Flights { get; set; }
    }
}