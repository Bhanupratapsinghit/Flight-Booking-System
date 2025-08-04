using FlightProject.DTOs;
using FlightProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightProject.Interfaces
{
    public interface IAirline
    {
        Task<IEnumerable<AirlineDto>> GetAllAirlinesAsync();
        Task<AirlineDto> GetAirlineByIdAsync(int airlineId);
        Task<bool> AddAirlineAsync(AddAirlineDto airlineDto);
        Task<bool> UpdateAirlineAsync(int airlineId, AirlineDto airlineDto);
        Task<bool> DeleteAirlineAsync(int airlineId);
    }
}
