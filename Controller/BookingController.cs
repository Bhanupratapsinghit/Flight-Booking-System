using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightProject.Interfaces;
using FlightProject.Models;
using FlightProject.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FlightProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBooking _booking;

        public BookingController(IBooking booking)
        {
            _booking = booking;
        }

        // ✅ Create a new booking (Uses full Booking model)
        [HttpPost("Book-Flight-As-Per-Your-Choice")]
        public async Task<IActionResult> BookFlight(int flightId, string userId, List<int> passengerIds)
        {
            try
            {
                var booking = await _booking.CreateBookingAsync(flightId, userId, passengerIds);
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingId }, new { BookingId = booking.BookingId, Message = "Flight booked successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // ✅ Get a booking by ID (Uses full Booking model)
        [HttpGet("Get-Booking-Details/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var booking = await _booking.GetBookingByIdAsync(id);
                if (booking == null)
                    return NotFound(new { Message = "Booking not found" });

                return Ok(booking); // Returns full Booking model
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        // ✅ Get all bookings (Uses List<BookingDto>)
        
        [HttpGet("Get-All-Bookings-Details")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _booking.GetAllBookingsAsync(); // Must return List<BookingDto>
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        // ✅ Update Booking (Uses BookingDto)
        [HttpPut("Update-Booking-Details/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDto updatedBooking)
        {
            if (updatedBooking == null)
                return BadRequest(new { Message = "Invalid booking details." });

            try
            {
                var updated = await _booking.UpdateBookingAsync(id, updatedBooking);
                if (updated == null)
                    return NotFound(new { Message = $"No booking found with ID {id}" });

                return Ok(updated); // ✅ Return updated BookingDto
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        // ✅ Cancel (Delete) Booking (Uses BookingDto)
        [Authorize(Roles ="Admin")]
        [HttpDelete("Cancel-Booking-On-Request/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var result = await _booking.CancelBookingAsync(id);
                
                if (!result)
                    return NotFound(new { Message = "Booking not found or already cancelled." });

                return Ok(new { Message = "Booking cancelled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }
    }
}
