using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightProject.DTOS;
using FlightProject.Models;

namespace FlightProject.Interfaces
{
    public interface IFlight
    {
        Task<IEnumerable<Flight>> SearchFlightAsync(string departure, string destination, DateTime date);
        Task<Flight?> GetFlightByIdAsync(int id);
        Task<bool> UpdateFlightSeatsAsync(int flightId, int seatsBooked);
        Task<IEnumerable<Flight>> GetFlightsByAirlineAsync(string departure, string destination, DateTime date, string airlineName);
        
        // New CRUD Methods
        Task<Flight> AddFlightAsync(FlightDto flightDto);
        Task<bool> UpdateFlightAsync(int flightId,FlightDto flightDto);
        Task<bool> DeleteFlightAsync(int flightId);
    }
}
