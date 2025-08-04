using System.Collections.Generic;
using System.Threading.Tasks;
using FlightProject.DTOS;

namespace FlightProject.Interfaces
{
    public interface IPassenger
    {
        Task<IEnumerable<PassengerDto>> GetAllPassengersAsync();
        Task<PassengerDto> GetPassengerByIdAsync(int id);
        Task<PassengerDto> AddPassengerAsync(PassengerDto passengerDto);
        Task<bool> UpdatePassengerAsync(int id, PassengerDto passengerDto);
        Task<bool> DeletePassengerAsync(int id);
    }
}
