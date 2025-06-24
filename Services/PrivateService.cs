using API_Project.Data;
using API_Project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Services
{
    public class PrivateService
    {
        private readonly ApplicationDbContext _context;
        public PrivateService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomDTO>> GetRoomsByCinemaAsync(string nameCinema)
        {
            var room = await _context.Rooms
                .Include(r => r.Cinema)
                .Where(r => r.Cinema.CinemaName == nameCinema)
                .Select(r => new RoomDTO
                {
                    IDRoom = r.IDRoom,
                    CinemaID = r.CinemaID,
                    RoomName = r.RoomName,
                    RoomType = r.RoomType,
                    RoomImageURL = r.RoomImageURL,
                    Status = r.Status
                })
                .ToListAsync();
            return room;
        }

    }
}
