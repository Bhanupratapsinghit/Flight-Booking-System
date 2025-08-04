using FlightProject.DTOs;
using FlightProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightProject.Interfaces
{
    public interface IAirport
{
    Task<IEnumerable<AirportDto>> GetAllAirportsAsync();
    Task<AirportDto> GetAirportByCodeAsync(string airportCode);
    Task<bool> AddAirportAsync(AirportDto airportDto);
    Task<bool> UpdateAirportAsync(string airportCode, AirportDto airportDto);
    Task<bool> DeleteAirportAsync(string airportCode);
}

}
