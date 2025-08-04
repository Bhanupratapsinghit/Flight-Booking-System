using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBookingSystem.Data;
using FlightProject.DTOS;
using FlightProject.Interfaces;
using FlightProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightProject.Repositories
{
    public class BookingRepository : IBooking
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Booking> CreateBookingAsync(int flightId, string userId, List<int> passengerIds)
        {
            try
            {
                if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
                    throw new KeyNotFoundException($"User with ID {userId} not found");

                var flight = await _dbContext.Flights
                    .Where(f => f.FlightId == flightId)
                    .Select(f => new { f.FlightId, f.Fare })
                    .FirstOrDefaultAsync();

                if (flight == null)
                    throw new KeyNotFoundException($"Flight with ID {flightId} not found");

                var existingPassengerIds = await _dbContext.Passengers
                    .Where(p => passengerIds.Contains(p.PassengerId))
                    .Select(p => p.PassengerId)
                    .ToListAsync();

                if (existingPassengerIds.Count != passengerIds.Count)
                    throw new KeyNotFoundException("Some passengers not found.");

                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                decimal totalFare = flight.Fare * passengerIds.Count; // Calculate total fare

                var booking = new Booking
                {
                    FlightId = flightId,
                    UserId = userId,
                    PaymentStatus = "Pending",
                    TotalFare = totalFare // Store total fare in booking
                };

                await _dbContext.Bookings.AddAsync(booking);
                await _dbContext.SaveChangesAsync();

                for (int i = 0; i < passengerIds.Count; i++)
                {
                    _dbContext.BookingPassengers.Add(new BookingPassenger
                    {
                        BookingId = booking.BookingId,
                        PassengerId = passengerIds[i],
                        SeatNumber = $"A{i + 1}"
                    });
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return booking;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while creating booking", ex);
            }
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            try
            {
                var booking = await _dbContext.Bookings.Include(b => b.BookingPassengers)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);
                if (booking == null) throw new KeyNotFoundException("Booking not found.");
                return booking;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving booking", ex);
            }
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            return await _dbContext.Bookings
                .Select(b => new BookingDto
                {
                    BookingId = b.BookingId,
                    FlightId = b.FlightId,
                    UserId = b.UserId,
                    PaymentStatus = b.PaymentStatus,
                    IsCancelled = b.IsCancelled,
                    CancellationDate = b.CancellationDate
                })
                .ToListAsync();
        }

        public async Task<BookingDto> UpdateBookingAsync(int bookingId, BookingDto updatedBooking)
        {
            var booking = await _dbContext.Bookings.FindAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            booking.FlightId = updatedBooking.FlightId;
            booking.UserId = updatedBooking.UserId;
            booking.PaymentStatus = updatedBooking.PaymentStatus;
            booking.IsCancelled = updatedBooking.IsCancelled;
            booking.CancellationDate = updatedBooking.IsCancelled ? (updatedBooking.CancellationDate ?? DateTime.UtcNow) : null;

            await _dbContext.SaveChangesAsync();

            return new BookingDto
            {
                BookingId = booking.BookingId,
                FlightId = booking.FlightId,
                UserId = booking.UserId,
                PaymentStatus = booking.PaymentStatus,
                IsCancelled = booking.IsCancelled,
                CancellationDate = booking.CancellationDate
            };
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _dbContext.Bookings.FindAsync(bookingId);
            if (booking == null)
                return false;

            booking.IsCancelled = true;
            booking.CancellationDate = DateTime.UtcNow;

            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
