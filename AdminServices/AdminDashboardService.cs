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
                //TicketsToday = await _db.invoice.CountAsync(h => h.NgayTao.Date == today),
                //TicketsThisMonth = await _db.invoice.CountAsync(h => h.NgayTao >= firstDayOfMonth),
                //TicketsThisYear = await _db.invoice.CountAsync(h => h.NgayTao >= firstDayOfYear),

                CurrentMovies = await _db.Movies.CountAsync(p => p.ReleaseDate <= today),
                UpcomingMovies = await _db.Movies.CountAsync(p => p.ReleaseDate > today),

                ActiveCinemas = await _db.Cinemas.CountAsync(r => r.IsActive == 0),
                TotalUsers = await _db.Users.CountAsync()
            };
        }
    }
}
