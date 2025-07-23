using API_Project.Data;
using API_Project.Models.DTOs;
using API_Project.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_Project.AdminServices
{
    public class SeatTypeInRoomService : ISeatTypeInRoomService
    {
        private readonly ApplicationDbContext _context;

        public SeatTypeInRoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SeatTypeInRoomDto>> GetAllAsync()
        {
            var query = from s in _context.SeatTypesInRoom
                        join r in _context.Rooms on s.RoomId equals r.IDRoom
                        join c in _context.Cinemas on s.CinemaId equals c.IDCinema
                        select new SeatTypeInRoomDto
                        {
                            Id = s.Id,
                            CinemaName = c.CinemaName,
                            RoomName = r.RoomName,
                            RowCode = s.RowCode,
                            SeatType = s.SeatType,
                            Price = s.Price
                        };

            return await query.ToListAsync();
        }

        public async Task<bool> CreateAsync(SeatTypeInRoomRequest request)
        {
            var entity = new SeatTypeInRoom
            {
                CinemaId = request.CinemaId,
                RoomId = request.RoomId,
                RowCode = request.RowCode,
                SeatType = request.SeatType,
                Price = request.Price
            };

            _context.SeatTypesInRoom.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(int id, SeatTypeInRoomRequest request)
        {
            var entity = await _context.SeatTypesInRoom.FindAsync(id);
            if (entity == null) return false;

            entity.CinemaId = request.CinemaId;
            entity.RoomId = request.RoomId;
            entity.RowCode = request.RowCode;
            entity.SeatType = request.SeatType;
            entity.Price = request.Price;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.SeatTypesInRoom.FindAsync(id);
            if (entity == null) return false;

            _context.SeatTypesInRoom.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
