using FlightProject.DTOS;
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
    public class CheckInController : ControllerBase
    {
        private readonly ICheckIn _checkInRepository;

        public CheckInController(ICheckIn checkInRepository)
        {
            _checkInRepository = checkInRepository;
        }

        // ✅ Check-in a Passenger
        [HttpPost("perform-CheckIn-For-Passengers/{bookingPassengerId}")]
        public async Task<IActionResult> CheckInPassenger(int bookingPassengerId)
        {
            try
            {
                var checkIn = await _checkInRepository.PerformCheckIn(bookingPassengerId);
                return Ok(new
                {
                    Message = "Check-in successful!",
                    CheckInDetails = checkIn
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        // ✅ Get All Check-Ins
        [HttpGet("Get-All-CheckIn-Passengers-Details")]
        public async Task<IActionResult> GetAllCheckIns()
        {
            try
            {
                var checkIns = await _checkInRepository.GetAllCheckInsWithDetails();
                return Ok(checkIns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        // ✅ Get Check-In by ID
        [HttpGet("Get-CheckIn-Passengers-Details{checkInId}")]
        public async Task<IActionResult> GetCheckInById(int checkInId)
        {
            try
            {
                var checkIn = await _checkInRepository.GetCheckInById(checkInId);
                return Ok(checkIn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
