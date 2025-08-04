using System;
using System.Threading.Tasks;
using FlightProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,User")]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment _payment;

        public PaymentController(IPayment payment)
        {
            _payment = payment;
        }

        [HttpPost("ProcessPayment-For-Flight-Confirmations")]
        public async Task<IActionResult> ProcessPayment(int bookingId, string paymentMethod, decimal amount)
        {
            if (bookingId <= 0)
            {
                return BadRequest(new { message = "Invalid booking ID." });
            }

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                return BadRequest(new { message = "Payment method is required." });
            }

            if (amount <= 0)
            {
                return BadRequest(new { message = "Invalid payment amount." });
            }

            try
            {
                var payment = await _payment.ProcessPayment(bookingId, paymentMethod, amount);
                return Ok(new
                {
                    message = "Payment successful",
                    payment
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred while processing payment." });
            }
        }
    }
}
