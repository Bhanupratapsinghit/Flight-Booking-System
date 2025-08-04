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
    public class AirlineController : ControllerBase
    {
        private readonly IAirline _airlineService;

        public AirlineController(IAirline airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpGet("Get-all-Airline-details")]
        public async Task<ActionResult<IEnumerable<AirlineDto>>> GetAllAirlines()
        {
            try
            {
                var airlines = await _airlineService.GetAllAirlinesAsync();
                return Ok(airlines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching airlines", error = ex.Message });
            }
        }

        [HttpGet("Get-all-Airline-details/{airlineId}")]
        public async Task<ActionResult<AirlineDto>> GetAirlineById(int airlineId)
        {
            try
            {
                var airline = await _airlineService.GetAirlineByIdAsync(airlineId);
                if (airline == null)
                    return NotFound(new { message = "Airline not found" });

                return Ok(airline);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the airline", error = ex.Message });
            }
        }

        [HttpPost("add-Airline-Details")]
        public async Task<IActionResult> AddAirline([FromBody] AddAirlineDto airlineDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _airlineService.AddAirlineAsync(airlineDto);
                if (!result)
                    return StatusCode(500, new { message = "Failed to add airline" });

                return Ok(new { message = "Airline added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the airline", error = ex.Message });
            }
        }

        [HttpPut("update-airline-details/{airlineId}")]
        public async Task<IActionResult> UpdateAirline(int airlineId, [FromBody] AirlineDto airlineDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _airlineService.UpdateAirlineAsync(airlineId, airlineDto);
                if (!result)
                    return NotFound(new { message = "Airline not found or update failed" });

                return Ok(new { message = "Airline updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the airline", error = ex.Message });
            }
        }

        [HttpDelete("delete-airline/{airlineId}")]
        public async Task<IActionResult> DeleteAirline(int airlineId)
        {
            try
            {
                var result = await _airlineService.DeleteAirlineAsync(airlineId);
                if (!result)
                    return NotFound(new { message = "Airline not found or delete failed" });

                return Ok(new { message = "Airline deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the airline", error = ex.Message });
            }
        }
    }
}
