using FlightBookingSystem.Data;
using FlightProject.DTOs;
using FlightProject.Interfaces;
using FlightProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.Repositories
{
    public class AirlineRepository : IAirline
    {
        private readonly ApplicationDbContext _context;

        public AirlineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AirlineDto>> GetAllAirlinesAsync()
        {
            try
            {
                return await _context.Airlines
                    .Select(a => new AirlineDto
                    {
                        AirlineId = a.AirlineId,
                        AirlineName = a.AirlineName,
                        ContactNumber = a.ContactNumber,
                        OperatingRegion = a.OperatingRegion
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching airlines: {ex.Message}");
                return new List<AirlineDto>(); // Return empty list on failure
            }
        }

        public async Task<AirlineDto> GetAirlineByIdAsync(int airlineId)
        {
            try
            {
                var airline = await _context.Airlines.FindAsync(airlineId);
                if (airline == null) return null;

                return new AirlineDto
                {
                    AirlineId = airline.AirlineId,
                    AirlineName = airline.AirlineName,
                    ContactNumber = airline.ContactNumber,
                    OperatingRegion = airline.OperatingRegion
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching airline by ID: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddAirlineAsync(AddAirlineDto airlineDto)
        {
            try
            {
                var airline = new Airline
                {
                    AirlineId=airlineDto.AirlineId,
                    AirlineName = airlineDto.AirlineName,
                    ContactNumber = airlineDto.ContactNumber,
                    OperatingRegion = airlineDto.OperatingRegion
                };

                await _context.Airlines.AddAsync(airline);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding airline: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAirlineAsync(int airlineId, AirlineDto airlineDto)
        {
            try
            {
                var airline = await _context.Airlines.FindAsync(airlineId);
                if (airline == null) return false;

                airline.AirlineName = airlineDto.AirlineName;
                airline.ContactNumber = airlineDto.ContactNumber;
                airline.OperatingRegion = airlineDto.OperatingRegion;

                _context.Airlines.Update(airline);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating airline: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAirlineAsync(int airlineId)
        {
            try
            {
                var airline = await _context.Airlines.FindAsync(airlineId);
                if (airline == null) return false;

                _context.Airlines.Remove(airline);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting airline: {ex.Message}");
                return false;
            }
        }
    }
}
