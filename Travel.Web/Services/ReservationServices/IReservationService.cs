using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities.Enums;

namespace Travel.Web.Services.ReservationServices
{
    public interface IReservationService
    {
        Task<ResultReservationDto> GetByIdAsync(string id);
        Task<List<ResultReservationDto>> GetAllAsync();
        Task<List<ResultReservationDto>> GetByUserIdAsync(string userId);
        Task<List<ResultReservationDto>> GetByTourIdAsync(string tourId, string tourDateId = null, string status = null);
        Task<string> CreateAsync(CreateReservationDto createReservationDto);
        Task UpdateStatusAsync(string id, ReservationStatus status);
    }
}
