using FlightProject.DTOS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightProject.Interfaces
{
    public interface ICheckIn
    {
        Task<CheckInDto> PerformCheckIn(int bookingPassengerId);
        Task<IEnumerable<CheckInDto>> GetAllCheckInsWithDetails();
        Task<CheckInDto> GetCheckInById(int checkInId);
    }
}
