using API_Project.Models.DTOs;

namespace API_Project.Services
{
    public interface ISeatLayoutCacheService
    {
        Task<Dictionary<string, SeatLayoutItemDto>> GetSeatLayoutAsync(int showtimeId);
    }

}