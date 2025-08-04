using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightProject.Interfaces;
using FlightProject.Models;
using FlightProject.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FlightProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlight _flight;

        public FlightController(IFlight flight)
        {
            _flight = flight;
        }

        // ✅ Search Flights
        [HttpGet("search-For-Flights-On-A-Particular-Route")]
        public async Task<IActionResult> SearchFlightAsync([FromQuery] string departureCode, [FromQuery] string destinationCode, [FromQuery] DateTime date)
        {
            try
            {
                if (string.IsNullOrEmpty(departureCode) || string.IsNullOrEmpty(destinationCode) || date == DateTime.MinValue)
                {
                    return BadRequest("Invalid search parameters. Please provide valid departure, destination, and date.");
                }

                var flights = await _flight.SearchFlightAsync(departureCode, destinationCode, date);

                if (flights == null || !flights.Any())
                {
                    return NotFound("No flights found for the given criteria.");
                }

                return Ok(flights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // ✅ Get Flight by ID
        [HttpGet("Get-Flight-Details/{id}")]
        public async Task<IActionResult> GetFlightByIdAsync(int id)
        {
            try
            {
                var flight = await _flight.GetFlightByIdAsync(id);

                if (flight == null)
                {
                    return NotFound($"Flight with ID {id} not found.");
                }

                return Ok(flight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // ✅ Get Flights by Airline Name
        [HttpGet("Get-Flight-Details-for-passengers-airline-choice")]
        public async Task<IActionResult> GetFlightsByAirlineAsync([FromQuery] string departureCode, [FromQuery] string destinationCode, [FromQuery] DateTime date, [FromQuery] string airlineName)
        {
            try
            {
                if (string.IsNullOrEmpty(airlineName))
                {
                    return BadRequest("Airline name must be provided.");
                }

                var flights = await _flight.GetFlightsByAirlineAsync(departureCode, destinationCode, date, airlineName);

                if (flights == null || !flights.Any())
                {
                    return NotFound($"No flights found for airline {airlineName} on the given route.");
                }

                return Ok(flights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // ✅ Add a New Flight (Admin-only)
         [HttpPost("add")]
         [Authorize(Roles ="Admin,User")]
        
         public async Task<IActionResult> AddFlight([FromBody] FlightDto flightDto)
      {
         try
         {
            var flight = await _flight.AddFlightAsync(flightDto);
            
            if (flight == null)
            {
                  return BadRequest("Failed to add flight.");
            }

            if (flightDto.DepartureDateTime >= flightDto.ArrivalDateTime)
            {
                return BadRequest(new { message = "Departure time must be before arrival time." });
            }

            var response = new
            {
                  message = "Flight added successfully",
                  flight = new FlightDto
                  {
                     AirlineId = flight.AirlineId,
                     FlightNumber = flight.FlightNumber,
                     DepartureDateTime = flight.DepartureDateTime,
                     ArrivalDateTime = flight.ArrivalDateTime,
                     OriginAirportCode = flight.OriginAirportCode,
                     DestinationAirportCode = flight.DestinationAirportCode,
                     AvailableSeats = flight.AvailableSeats,
                     Fare=flight.Fare
                  }
            };

            return StatusCode(201, response);
         }
         catch (Exception ex)
         {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
         }
      }




        // ✅ Update Flight
        [HttpPut("update-Flight-details/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateFlightAsync(int id, [FromBody] FlightDto flightDto)
        {
            if (flightDto.DepartureDateTime >= flightDto.ArrivalDateTime)
            {
                return BadRequest(new { message = "Departure time must be before arrival time." });
            }

            var success = await _flight.UpdateFlightAsync(id, flightDto);
            return success ? Ok(new { message = "Flight updated successfully." }) : NotFound(new { message = "Flight not found." });
        }

        // ✅ Delete Flight
        [HttpDelete("delete-Flight-Details/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var success = await _flight.DeleteFlightAsync(id);
            return success ? Ok(new { message = $"Flight with ID {id} deleted successfully." }) : NotFound(new { message = $"Flight with ID {id} not found." });
        }
    }
}
