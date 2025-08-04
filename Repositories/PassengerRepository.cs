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
    public class PassengerRepository : IPassenger
    {
        private readonly ApplicationDbContext _context;

        public PassengerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PassengerDto>> GetAllPassengersAsync()
        {
            try
            {
                return await _context.Passengers
                    .Select(p => new PassengerDto
                    {
                        PassengerId = p.PassengerId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Email = p.Email,
                        PassportNumber = p.PassportNumber
                    }).ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error retrieving passengers.");
            }
        }

        public async Task<PassengerDto> GetPassengerByIdAsync(int id)
        {
            try
            {
                var passenger = await _context.Passengers.FindAsync(id);
                if (passenger == null) return null;

                return new PassengerDto
                {
                    PassengerId = passenger.PassengerId,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    Email = passenger.Email,
                    PassportNumber = passenger.PassportNumber
                };
            }
            catch (Exception)
            {
                throw new Exception("Error retrieving the passenger.");
            }
        }

       public async Task<PassengerDto> AddPassengerAsync(PassengerDto passengerDto)
        {
            try
            {
                var passenger = new Passenger
                {
                    UserId = passengerDto.UserId, // ✅ Ensure UserId is set
                    FirstName = passengerDto.FirstName,
                    LastName = passengerDto.LastName,
                    Email = passengerDto.Email,
                    PassportNumber = passengerDto.PassportNumber
                };

                _context.Passengers.Add(passenger);
                await _context.SaveChangesAsync();

                passengerDto.PassengerId = passenger.PassengerId;
                return passengerDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.InnerException?.Message ?? ex.Message}\n{ex.StackTrace}");
                throw new Exception("Database Error: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }



        public async Task<bool> UpdatePassengerAsync(int id, PassengerDto passengerDto)
        {
            try
            {
                var passenger = await _context.Passengers.FindAsync(id);
                if (passenger == null) return false;

                passenger.FirstName = passengerDto.FirstName;
                passenger.LastName = passengerDto.LastName;
                passenger.Email = passengerDto.Email;
                passenger.PassportNumber = passengerDto.PassportNumber;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error updating the passenger.");
            }
        }

        public async Task<bool> DeletePassengerAsync(int id)
        {
            try
            {
                var passenger = await _context.Passengers.FindAsync(id);
                if (passenger == null) return false;

                _context.Passengers.Remove(passenger);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error deleting the passenger.");
            }
        }
    }
}
