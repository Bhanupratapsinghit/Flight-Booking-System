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
    public class AirportRepository : IAirport
    {
        private readonly ApplicationDbContext _context;

        public AirportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all airports and return DTOs
        public async Task<IEnumerable<AirportDto>> GetAllAirportsAsync()
        {
            return await _context.Airports
                .Select(a => new AirportDto
                {
                    AirportCode = a.AirportCode,
                    AirportName = a.AirportName,
                    Location = a.Location,
                    Facilities = a.Facilities
                }).ToListAsync();
        }

        // ✅ Get airport by code and return DTO
        public async Task<AirportDto> GetAirportByCodeAsync(string airportCode)
        {
            var airport = await _context.Airports
                .FirstOrDefaultAsync(a => a.AirportCode == airportCode);

            if (airport == null) return null;

            return new AirportDto
            {
                AirportCode = airport.AirportCode,
                AirportName = airport.AirportName,
                Location = airport.Location,
                Facilities = airport.Facilities
            };
        }

        // ✅ Add new airport using DTO
        public async Task<bool> AddAirportAsync(AirportDto airportDto)
        {
            try
            {
                var airport = new Airport
                {
                    AirportCode = airportDto.AirportCode,
                    AirportName = airportDto.AirportName,
                    Location = airportDto.Location,
                    Facilities = airportDto.Facilities
                };

                await _context.Airports.AddAsync(airport);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ✅ Update existing airport using DTO
        public async Task<bool> UpdateAirportAsync(string airportCode, AirportDto airportDto)
        {
            try
            {
                var airport = await _context.Airports.FirstOrDefaultAsync(a => a.AirportCode == airportCode);
                if (airport == null) return false; // Airport not found

                // Update fields
                airport.AirportName = airportDto.AirportName;
                airport.Location = airportDto.Location;
                airport.Facilities = airportDto.Facilities;

                _context.Airports.Update(airport);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ✅ Delete airport by code
        public async Task<bool> DeleteAirportAsync(string airportCode)
        {
            try
            {
                var airport = await _context.Airports.FirstOrDefaultAsync(a => a.AirportCode == airportCode);
                if (airport == null) return false;

                _context.Airports.Remove(airport);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
