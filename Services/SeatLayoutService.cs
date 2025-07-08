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

        public async Task<object> GetFullSeatLayoutWithPricesAsync(string idLayout, int idSuatChieu)
        {
            if (_env.WebRootPath == null)
                throw new InvalidOperationException("WebRootPath is not set.");
            if (string.IsNullOrEmpty(idLayout))
                throw new ArgumentException("idLayout cannot be null or empty", nameof(idLayout));

            // 1. Truy xuất suất chiếu để lấy MaRap, MaPhong, Type
            var suatChieu = await _dbContext.Showtimes
                .FirstOrDefaultAsync(s => s.MaSuatChieu == idSuatChieu);

            if (suatChieu == null)
                throw new Exception($"Không tìm thấy suất chiếu với id = {idSuatChieu}");

            int maRap = suatChieu.MaRap;
            int maPhong = suatChieu.PhongChieu;
            int typeSuatChieu = suatChieu.TypeSuatChieu;

            // 2. Đọc layout từ file
            string layoutPath = Path.Combine(_env.WebRootPath, "data", "seat-layout", $"{maRap}_{idLayout}.json");
            if (!File.Exists(layoutPath))
                throw new FileNotFoundException($"Layout file not found: {layoutPath}");

            string layoutJson = await File.ReadAllTextAsync(layoutPath);
            var layoutData = JsonSerializer.Deserialize<SeatLayoutWrapper>(layoutJson);

            // 3. Ghế đã thanh toán
            var confirmedSeats = await _dbContext.BookedSeats
                .Where(s => s.IdShowtime == idSuatChieu && s.status == "Đã thanh toán")
                .Select(s => s.IdSeat)
                .ToListAsync();

            // 4. Ghế đang giữ
            string redisKey = $"suatchieu:{idSuatChieu}:held_seats";
            var heldSeats = await _redis.SetMembersAsync(redisKey);
            var heldList = heldSeats.Select(v => v.ToString()).ToList();

            // 5. Lấy giá suất chiếu
            var loaiSuat = await _dbContext.ShowtimeTypes
                .FirstOrDefaultAsync(x => x.Id == typeSuatChieu);

            if (loaiSuat == null)
                throw new Exception($"Không tìm thấy loại suất chiếu với type = {typeSuatChieu}");

            long giaSuatChieu = loaiSuat.Price;

            var seatPrices = await _dbContext.SeatTypesInRooms
                .Where(x => x.RoomId == maPhong && x.CinemaId == maRap)
                .ToListAsync();

            // 7. Tính giá mỗi dãy = giá ghế + giá suất chiếu
            var priceByRow = seatPrices.ToDictionary(
                x => x.RowCode, 
                x => x.Price + giaSuatChieu
            );

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
