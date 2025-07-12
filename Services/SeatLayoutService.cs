using API_Project.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace API_Project.Services
{
    public class SeatLayoutService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDatabase _redis;

        public SeatLayoutService(IWebHostEnvironment env, ApplicationDbContext dbContext, IConnectionMultiplexer redis)
        {
            _env = env;
            _dbContext = dbContext;
            _redis = redis.GetDatabase();
        }

        public async Task<object> GetFullSeatLayoutWithPricesAsync(int idSuatChieu)
        {
            if (_env.WebRootPath == null)
                throw new InvalidOperationException("WebRootPath is not set.");

            // 1. Lấy suất chiếu
            var suatChieu = await _dbContext.Showtimes
                .FirstOrDefaultAsync(s => s.MaSuatChieu == idSuatChieu);

            if (suatChieu == null)
                throw new Exception($"Không tìm thấy suất chiếu với id = {idSuatChieu}");

            int maRap = suatChieu.MaRap;
            int maPhong = suatChieu.PhongChieu;
            int typeSuatChieu = suatChieu.TypeSuatChieu;

            // 2. Lấy layout ID từ phòng
            var phong = await _dbContext.Rooms
                .FirstOrDefaultAsync(p => p.IDRoom == maPhong);

            if (phong == null)
                throw new Exception($"Không tìm thấy phòng chiếu với id = {maPhong}");

            if (string.IsNullOrEmpty(phong.id_layout))
                throw new Exception($"Phòng chiếu id = {maPhong} chưa có layout.");

            // 3. Đọc file layout JSON
            string layoutPath = Path.Combine(_env.WebRootPath, "data", "seat-layout", $"{phong.id_layout}.json");

            if (!File.Exists(layoutPath))
                throw new FileNotFoundException($"Layout file not found: {layoutPath}");

            string layoutJson = await File.ReadAllTextAsync(layoutPath);
            var layoutData = JsonSerializer.Deserialize<SeatLayoutWrapper>(layoutJson);

            // 4. Ghế đã thanh toán
            var confirmedSeats = await _dbContext.BookedSeats
                .Where(s => s.IdShowtime == idSuatChieu && s.status == "Đã thanh toán")
                .Select(s => s.IdSeat)
                .ToListAsync();

            // 5. Ghế đang giữ (từ Redis)
            string redisKey = $"suatchieu:{idSuatChieu}:held_seats";
            var heldSeats = await _redis.SetMembersAsync(redisKey);
            var heldList = heldSeats.Select(v => v.ToString()).ToList();

            // 6. Giá suất chiếu
            var loaiSuat = await _dbContext.ShowtimeTypes
                .FirstOrDefaultAsync(x => x.Id == typeSuatChieu);

            if (loaiSuat == null)
                throw new Exception($"Không tìm thấy loại suất chiếu với type = {typeSuatChieu}");

            long giaSuatChieu = loaiSuat.Price;

            // 7. Giá ghế theo dãy
            var seatPrices = await _dbContext.SeatTypesInRooms
                .Where(x => x.RoomId == maPhong && x.CinemaId == maRap)
                .ToListAsync();

            var priceByRow = seatPrices.ToDictionary(
                x => x.RowCode,
                x => x.Price + giaSuatChieu
            );

            // 8. Trả về
            return new
            {
                layout = layoutData.layout,
                confirmed = confirmedSeats,
                held = heldList,
                prices = priceByRow
            };
        }




        // Helper class để map JSON
        private class SeatLayoutWrapper
        {
            public List<List<string?>> layout { get; set; } = new();
        }
    }
}
