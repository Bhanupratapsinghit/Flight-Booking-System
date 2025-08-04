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
    public class FlightRepository : IFlight
    {
        private readonly ApplicationDbContext _dbcontext;

        public FlightRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Flight>> SearchFlightAsync(string departure, string destination, DateTime date)
        {
            try
            {
                if (string.IsNullOrEmpty(departure) || string.IsNullOrEmpty(destination) || date == DateTime.MinValue)
                    return new List<Flight>();

                return await _dbcontext.Flights
                    .Include(f => f.Airline)
                    .Where(f => f.OriginAirportCode == departure &&
                                f.DestinationAirportCode == destination &&
                                f.DepartureDateTime.Date == date.Date &&
                                f.AvailableSeats > 0)
                    .Select(f => new Flight
                    {
                        FlightId = f.FlightId,
                        AirlineId = f.AirlineId,
                        Airline = f.Airline,
                        FlightNumber = f.FlightNumber,
                        DepartureDateTime = f.DepartureDateTime,
                        ArrivalDateTime = f.ArrivalDateTime,
                        OriginAirportCode = f.OriginAirportCode,
                        DestinationAirportCode = f.DestinationAirportCode,
                        AvailableSeats = f.AvailableSeats,
                        Fare = f.Fare // Added fare to the response
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving flights: " + ex.Message, ex);
            }
        }

        public async Task<Flight?> GetFlightByIdAsync(int id)
        {
            try
            {
                return await _dbcontext.Flights
                    .Include(f => f.Airline)
                    .Select(f => new Flight
                    {
                        FlightId = f.FlightId,
                        AirlineId = f.AirlineId,
                        Airline = f.Airline,
                        FlightNumber = f.FlightNumber,
                        DepartureDateTime = f.DepartureDateTime,
                        ArrivalDateTime = f.ArrivalDateTime,
                        OriginAirportCode = f.OriginAirportCode,
                        DestinationAirportCode = f.DestinationAirportCode,
                        AvailableSeats = f.AvailableSeats,
                        Fare = f.Fare // Added fare to the response
                    })
                    .FirstOrDefaultAsync(f => f.FlightId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving flight with ID {id}: " + ex.Message, ex);
            }
        }

        public async Task<bool> UpdateFlightSeatsAsync(int flightId, int seatsBooked)
        {
            try
            {
                var flight = await _dbcontext.Flights.FindAsync(flightId);
                if (flight == null || flight.AvailableSeats < seatsBooked)
                    return false;

                flight.AvailableSeats -= seatsBooked;
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating flight seats: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Flight>> GetFlightsByAirlineAsync(string departure, string destination, DateTime date, string airlineName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(airlineName))
                {
                    throw new ArgumentNullException(nameof(airlineName), "Airline name must be provided.");
                }

                DateTime startOfDay = date.Date;
                DateTime endOfDay = date.Date.AddDays(1);

                var flights = await _dbcontext.Flights
                    .Include(f => f.Airline)
                    .Include(f => f.OriginAirport)
                    .Include(f => f.DestinationAirport)
                    .Where(f => f.OriginAirportCode == departure &&
                                f.DestinationAirportCode == destination &&
                                f.Airline.AirlineName == airlineName &&
                                f.DepartureDateTime >= startOfDay &&
                                f.DepartureDateTime < endOfDay &&
                                f.AvailableSeats > 0)
                    .OrderBy(f => f.DepartureDateTime)
                    .Select(f => new Flight
                    {
                        FlightId = f.FlightId,
                        AirlineId = f.AirlineId,
                        Airline = f.Airline,
                        FlightNumber = f.FlightNumber,
                        DepartureDateTime = f.DepartureDateTime,
                        ArrivalDateTime = f.ArrivalDateTime,
                        OriginAirportCode = f.OriginAirportCode,
                        DestinationAirportCode = f.DestinationAirportCode,
                        AvailableSeats = f.AvailableSeats,
                        Fare = f.Fare // Added fare to the response
                    })
                    .ToListAsync();

                return flights;
            }
            catch (ArgumentNullException argEx)
            {
                throw new ArgumentException("Invalid input: " + argEx.Message, argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving flights by airline: " + ex.Message, ex);
            }
        }

        public async Task<Flight> AddFlightAsync(FlightDto flightDto)
        {
            try
            {
                var flight = new Flight
                {
                    AirlineId = flightDto.AirlineId,
                    FlightNumber = flightDto.FlightNumber,
                    DepartureDateTime = flightDto.DepartureDateTime,
                    ArrivalDateTime = flightDto.ArrivalDateTime,
                    OriginAirportCode = flightDto.OriginAirportCode,
                    DestinationAirportCode = flightDto.DestinationAirportCode,
                    AvailableSeats = flightDto.AvailableSeats,
                    Fare = flightDto.Fare // Added fare to the new flight
                };

                await _dbcontext.Flights.AddAsync(flight);
                await _dbcontext.SaveChangesAsync();
                return flight;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding flight: " + ex.Message, ex);
            }
        }

        public async Task<bool> UpdateFlightAsync(int flightId, FlightDto flightDto)
        {
            try
            {
                var existingFlight = await _dbcontext.Flights.FindAsync(flightId);
                if (existingFlight == null)
                    return false;

                existingFlight.AirlineId = flightDto.AirlineId;
                existingFlight.FlightNumber = flightDto.FlightNumber;
                existingFlight.DepartureDateTime = flightDto.DepartureDateTime;
                existingFlight.ArrivalDateTime = flightDto.ArrivalDateTime;
                existingFlight.OriginAirportCode = flightDto.OriginAirportCode;
                existingFlight.DestinationAirportCode = flightDto.DestinationAirportCode;
                existingFlight.AvailableSeats = flightDto.AvailableSeats;
                existingFlight.Fare = flightDto.Fare; // Added fare update

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating flight: " + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteFlightAsync(int flightId)
        {
            try
            {
                var flight = await _dbcontext.Flights.FindAsync(flightId);
                if (flight == null)
                    return false;

                _dbcontext.Flights.Remove(flight);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting flight: " + ex.Message, ex);
            }
        }
    }
}