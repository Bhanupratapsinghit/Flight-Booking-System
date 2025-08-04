using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightProject.DTOS;
using FlightProject.Models;

namespace FlightProject.Interfaces
{
    public interface IBooking
    {
        Task<Booking> CreateBookingAsync(int flightId,string userId,List<int> passengerIds);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();  // ✅ Returns DTO
        Task<BookingDto> UpdateBookingAsync(int bookingId, BookingDto updatedBooking);  // ✅ Uses DTO
        Task<bool> CancelBookingAsync(int bookingId);  // ✅ Delete uses DTO

        

    }
}