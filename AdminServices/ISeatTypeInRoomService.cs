using API_Project.Models.DTOs;

namespace API_Project.AdminServices
{
    public interface ISeatTypeInRoomService
    {
        Task<List<SeatTypeInRoomDto>> GetAllAsync();
        Task<bool> CreateAsync(SeatTypeInRoomRequest request);
        Task<bool> UpdateAsync(int id, SeatTypeInRoomRequest request);
        Task<bool> DeleteAsync(int id);
    }


}
