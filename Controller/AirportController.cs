using FlightProject.DTOs;
using FlightProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AirportController : ControllerBase
    {
        private readonly IAirport _airportRepository;

        public AirportController(IAirport airportRepository)
        {
            _airportRepository = airportRepository;
        }

        // ✅ GET: api/airport
        [HttpGet("Get-All-Airport-Details")]
        public async Task<ActionResult<IEnumerable<AirportDto>>> GetAllAirports()
        {
            try
            {
                var airports = await _airportRepository.GetAllAirportsAsync();
                return Ok(airports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while retrieving airports.", 
                    Error = ex.Message 
                });
            }
        }

        // ✅ GET: api/airport/{airportCode}
        [HttpGet("Get-Airport-Details/{airportCode}")]
        public async Task<ActionResult<AirportDto>> GetAirportByCode([FromRoute] string airportCode)
        {
            try
            {
                var airport = await _airportRepository.GetAirportByCodeAsync(airportCode);
                if (airport == null)
                    return NotFound(new { Message = "Airport not found" });

                return Ok(airport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while retrieving the airport.", 
                    Error = ex.Message 
                });
            }
        }

        // ✅ POST: api/airport
        [HttpPost("Add-Airport-Details")]
        public async Task<IActionResult> AddAirport([FromBody] AirportDto airportDto)
        {
            try
            {
                if (airportDto == null)
                    return BadRequest(new { Message = "Invalid airport data" });

                var result = await _airportRepository.AddAirportAsync(airportDto);
                if (!result)
                    return StatusCode(500, new { Message = "Failed to add airport" });

                return CreatedAtAction(nameof(GetAirportByCode), new { airportCode = airportDto.AirportCode }, airportDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while adding the airport.", 
                    Error = ex.Message 
                });
            }
        }

        // ✅ PUT: api/airport/{airportCode}
        [HttpPut("update-Airport-Details/{airportCode}")]
        public async Task<IActionResult> UpdateAirport([FromRoute] string airportCode, [FromBody] AirportDto airportDto)
        {
            try
            {
                if (airportDto == null)
                    return BadRequest(new { Message = "Invalid airport data" });

                var result = await _airportRepository.UpdateAirportAsync(airportCode, airportDto);
                if (!result)
                    return NotFound(new { Message = "Airport not found or update failed" });

                return Ok(new { Message = "Airport updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while updating the airport.", 
                    Error = ex.Message 
                });
            }
        }

        // ✅ DELETE: api/airport/{airportCode}
        [HttpDelete("delete-Airport-Details/{airportCode}")]
        public async Task<IActionResult> DeleteAirport([FromRoute] string airportCode)
        {
            try
            {
                var result = await _airportRepository.DeleteAirportAsync(airportCode);
                if (!result)
                    return NotFound(new { Message = "Airport not found or delete failed" });

                return Ok(new { Message = "Airport deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while deleting the airport.", 
                    Error = ex.Message 
                });
            }
        }
    }
}
