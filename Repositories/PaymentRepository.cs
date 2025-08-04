using System;
using System.Threading.Tasks;
using FlightBookingSystem.Data;
using FlightProject.Interfaces;
using FlightProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightProject.Repositories
{
    public class PaymentRepository : IPayment
    {
        private readonly ApplicationDbContext _dbcontext;

        public PaymentRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Payment> ProcessPayment(int bookingId, string paymentMethod, decimal amount)
        {
            if (bookingId <= 0)
                throw new ArgumentException("Invalid booking ID.");

            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("Payment method is required.");

            if (amount <= 0)
                throw new ArgumentException("Invalid payment amount.");

            var booking = await _dbcontext.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException("Booking not found.");
            }

            if (booking.PaymentStatus == "Completed")
            {
                throw new InvalidOperationException("Payment has already been processed for this booking.");
            }

            decimal totalFare = booking.TotalFare;
            if (amount < totalFare)
            {
                throw new InvalidOperationException($"Insufficient payment. Total fare is {totalFare}, but received {amount}.");
            }

            var payment = new Payment
            {
                BookingId = bookingId,
                PaymentMethod = paymentMethod,
                Amount = amount,
                TransactionDateTime = DateTime.UtcNow
            };

            await _dbcontext.Payments.AddAsync(payment);
            booking.PaymentStatus = "Completed";
            _dbcontext.Bookings.Update(booking);
            await _dbcontext.SaveChangesAsync();

            return payment;
        }
    }
}
