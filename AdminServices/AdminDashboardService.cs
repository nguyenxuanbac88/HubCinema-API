using API_Project.Data;
using API_Project.Models;
using API_Project.Models.DTOs;
using API_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ApplicationDbContext _db;

        public AdminDashboardService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfYear = new DateTime(today.Year, 1, 1);

            return new DashboardSummaryDto
            {
                // Thống kê theo vé đã thanh toán
                TicketsToday = await _db.BookedSeats.CountAsync(bs =>
                    bs.CreatedAt.Date == today && bs.Status == "Đã thanh toán"),

                TicketsThisMonth = await _db.BookedSeats.CountAsync(bs =>
                    bs.CreatedAt >= firstDayOfMonth && bs.Status == "Đã thanh toán"),

                TicketsThisYear = await _db.BookedSeats.CountAsync(bs =>
                    bs.CreatedAt >= firstDayOfYear && bs.Status == "Đã thanh toán"),

                // Phim và rạp
                CurrentMovies = await _db.Movies.CountAsync(p => p.ReleaseDate <= today),
                UpcomingMovies = await _db.Movies.CountAsync(p => p.Status == 0),
                ActiveCinemas = await _db.Cinemas.CountAsync(r => r.IsActive == 0),
                TotalUsers = await _db.Users.CountAsync()
            };
        }

        public async Task<List<ChartDataPoint>> GetDailyTicketSalesAsync()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var rawData = await _db.BookedSeats
                .Where(x => x.CreatedAt >= startOfMonth && x.Status == "Đã thanh toán")
                .ToListAsync(); // Lấy toàn bộ ra trước

            var data = rawData
                .GroupBy(x => x.CreatedAt.Date)
                .Select(g => new ChartDataPoint
                {
                    Label = g.Key.ToString("dd/MM"),
                    Value = g.Count()
                })
                .OrderBy(x => DateTime.ParseExact(x.Label, "dd/MM", null))
                .ToList();

            return data;
        }


        public async Task<List<ChartDataPoint>> GetTicketSalesByCinemaAsync()
        {
            var data = await _db.BookedSeats
                .Where(x => x.Status == "Đã thanh toán")
                .Join(_db.Showtimes, bs => bs.ShowtimeId, st => st.MaSuatChieu, (bs, st) => new { bs, st })
                .Join(_db.Rooms, temp => temp.st.PhongChieu, r => r.IDRoom, (temp, r) => new { temp.bs, Room = r })
                .Join(_db.Cinemas, temp => temp.Room.CinemaID, c => c.IDCinema, (temp, c) => new { temp.bs, Cinema = c })
                .GroupBy(x => x.Cinema.CinemaName)
                .Select(g => new ChartDataPoint
                {
                    Label = g.Key,
                    Value = g.Count()
                })
                .OrderByDescending(x => x.Value)
                .ToListAsync();

            return data;
        }
        public async Task<DashboardFullDto> GetFullDashboardAsync()
        {
            var summary = await GetDashboardSummaryAsync();
            var dailySales = await GetDailyTicketSalesAsync();
            var cinemaSales = await GetTicketSalesByCinemaAsync();

            return new DashboardFullDto
            {
                Summary = summary,
                DailySales = dailySales,
                CinemaSales = cinemaSales
            };
        }

    }
}
