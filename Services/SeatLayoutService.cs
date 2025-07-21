using API_Project.Data;
using API_Project.Models.DTOs;
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

            //Lấy suất chiếu
            var suatChieu = await _dbContext.Showtimes
                .FirstOrDefaultAsync(s => s.MaSuatChieu == idSuatChieu);

            if (suatChieu == null)
                throw new Exception($"Không tìm thấy suất chiếu với id = {idSuatChieu}");

            int maRap = suatChieu.MaRap;
            int maPhong = suatChieu.PhongChieu;
            int typeSuatChieu = suatChieu.TypeSuatChieu;

            //Lấy layout ID từ phòng
            var phong = await _dbContext.Rooms
                .FirstOrDefaultAsync(p => p.IDRoom == maPhong);

            if (phong == null)
                throw new Exception($"Không tìm thấy phòng chiếu với id = {maPhong}");

            if (string.IsNullOrEmpty(phong.id_layout))
                throw new Exception($"Phòng chiếu id = {maPhong} chưa có layout.");

            //Đọc file layout JSON
            string layoutPath = Path.Combine(_env.WebRootPath, "data", "seat-layout", $"{phong.id_layout}.json");

            if (!File.Exists(layoutPath))
                throw new FileNotFoundException($"Layout file not found: {layoutPath}");

            string layoutJson = await File.ReadAllTextAsync(layoutPath);
            var layoutData = JsonSerializer.Deserialize<SeatLayoutWrapper>(layoutJson);

            var confirmedSeats = await _dbContext.BookedSeats
                .Where(s => s.ShowtimeId == idSuatChieu && s.Status == "Đã thanh toán")
                .Select(s => s.SeatCode)
                .ToListAsync();

            //Ghế đang giữ (từ Redis)
            string redisKey = $"suatchieu:{idSuatChieu}:held_seats";
            var heldSeats = await _redis.SetMembersAsync(redisKey);
            var heldList = heldSeats.Select(v => v.ToString()).ToList();

            //Giá suất chiếu
            var loaiSuat = await _dbContext.ShowtimeTypes
                .FirstOrDefaultAsync(x => x.Id == typeSuatChieu);

            if (loaiSuat == null)
                throw new Exception($"Không tìm thấy loại suất chiếu với type = {typeSuatChieu}");

            long giaSuatChieu = loaiSuat.Price;

            //Giá ghế theo dãy
            var seatPrices = await _dbContext.SeatTypesInRooms
                .Where(x => x.RoomId == maPhong && x.CinemaId == maRap)
                .ToListAsync();
            var priceByRow = seatPrices.ToDictionary(
                x => x.RowCode,
                x => new
                {
                    SeatType = x.SeatType, 
                    Price = x.Price + giaSuatChieu
                }
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
        public async Task<bool> SaveCustomSeatLayoutAsync(CustomSeatLayout request, string rootPath)
        {
            if (string.IsNullOrWhiteSpace(request.FileName) || request.Layout == null || request.Layout.Count == 0)
                return false;

            var layoutWrapper = new { layout = request.Layout };

            string folder = Path.Combine(rootPath, "data", "seat-layout");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filePath = Path.Combine(folder, $"{request.FileName}.json");
            string json = JsonSerializer.Serialize(layoutWrapper, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(filePath, json);
            return true;
        }




        // Helper class để map JSON
        private class SeatLayoutWrapper
        {
            public List<List<string?>> layout { get; set; } = new();
        }
    }
}
