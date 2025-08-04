using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightProject.DTOS;
using FlightProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FlightProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class PassengerController : ControllerBase
    {
        private readonly IPassenger _passengerRepository;

        public PassengerController(IPassenger passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        [HttpGet("Get-All-Passengers-Details")]
        public async Task<IActionResult> GetAllPassengers()
        {
            try
            {
                var passengers = await _passengerRepository.GetAllPassengersAsync();
                return Ok(passengers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Get-Passengers-Details/{id}")]
        public async Task<IActionResult> GetPassengerById(int id)
        {
            try
            {
                var passenger = await _passengerRepository.GetPassengerByIdAsync(id);
                if (passenger == null) return NotFound("Passenger not found.");
                return Ok(passenger);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Add-Any-New-Passengers")]
public async Task<IActionResult> AddPassenger([FromBody] PassengerDto passengerDto)
{
    if (!ModelState.IsValid) 
        return BadRequest(ModelState);

    try
    {
        var addedPassenger = await _passengerRepository.AddPassengerAsync(passengerDto);
         return CreatedAtAction(nameof(GetPassengerById), new { id = addedPassenger.PassengerId }, 
        new { message = "Passenger added successfully", passenger = addedPassenger });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}"); // 🔥 Print error to console/logs
        return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding the passenger: {ex.Message}");
    }
}


        [HttpPut("Update-Existing-Passengers/{id}")]
        public async Task<IActionResult> UpdatePassenger(int id, [FromBody] PassengerDto passengerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = await _passengerRepository.UpdatePassengerAsync(id, passengerDto);
                if (!updated) return NotFound("Passenger not found.");
                return Ok("Passenger updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete-Passenger-Details/{id}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            try
            {
                var deleted = await _passengerRepository.DeletePassengerAsync(id);
                if (!deleted) return NotFound("Passenger not found.");
                return Ok("Passenger deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
