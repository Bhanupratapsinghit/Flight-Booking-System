using FlightProject.DTOS;
using FlightBookingSystem.Data;
using FlightProject.Interfaces;
using FlightProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.Repositories
{
    public class CheckInRepository : ICheckIn
    {
        private readonly ApplicationDbContext _context;

        public CheckInRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CheckInDto> PerformCheckIn(int bookingPassengerId)
        {
            try
            {
                var checkIn = await _context.CheckIns.FirstOrDefaultAsync(c => c.BookingPassengerId == bookingPassengerId);

                if (checkIn == null)
                {
                    checkIn = new CheckIn
                    {
                        BookingPassengerId = bookingPassengerId,
                        HasCheckedIn = true,
                        CheckInTime = DateTime.UtcNow
                    };
                    _context.CheckIns.Add(checkIn);
                }
                else if (checkIn.HasCheckedIn)
                {
                    throw new InvalidOperationException("Passenger has already checked in!");
                }
                else
                {
                    checkIn.HasCheckedIn = true;
                    checkIn.CheckInTime = DateTime.UtcNow;
                    _context.CheckIns.Update(checkIn);
                }

                await _context.SaveChangesAsync();

                return new CheckInDto
                {
                    CheckInId = checkIn.CheckInId,
                    BookingPassengerId = checkIn.BookingPassengerId,
                    CheckInTime = checkIn.CheckInTime,
                    HasCheckedIn = checkIn.HasCheckedIn
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while performing check-in: " + ex.Message);
            }
        }

        public async Task<IEnumerable<CheckInDto>> GetAllCheckInsWithDetails()
        {
            try
            {
                return await _context.CheckIns
                    .Include(c => c.BookingPassenger)
                    .Select(c => new CheckInDto
                    {
                        CheckInId = c.CheckInId,
                        BookingPassengerId = c.BookingPassengerId,
                        CheckInTime = c.CheckInTime,
                        HasCheckedIn = c.HasCheckedIn
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching check-ins: " + ex.Message);
            }
        }

        public async Task<CheckInDto> GetCheckInById(int checkInId)
        {
            try
            {
                var checkIn = await _context.CheckIns
                    .Include(c => c.BookingPassenger)
                    .FirstOrDefaultAsync(c => c.CheckInId == checkInId);

                if (checkIn == null)
                {
                    throw new KeyNotFoundException("Check-in record not found.");
                }

                return new CheckInDto
                {
                    CheckInId = checkIn.CheckInId,
                    BookingPassengerId = checkIn.BookingPassengerId,
                    CheckInTime = checkIn.CheckInTime,
                    HasCheckedIn = checkIn.HasCheckedIn
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching check-in by ID: " + ex.Message);
            }
        }
    }
}
