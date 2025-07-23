using API_Project.Data;
using API_Project.Models.DTOs;
using API_Project.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Project.AdminServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceDetailDto>> GetAllInvoicesAsync()
        {
            var invoices = await _context.Invoices.ToListAsync();
            var result = new List<InvoiceDetailDto>();

            foreach (var invoice in invoices)
            {
                // Lấy danh sách ghế đã đặt
                var bookedSeats = await _context.BookedSeats
                    .Where(b => b.InvoiceId == invoice.IdInvoice)
                    .ToListAsync();

                // Lấy ShowtimeId đầu tiên để truy ngược về phim
                var firstShowtimeId = bookedSeats.FirstOrDefault()?.ShowtimeId;

                Showtime? showtime = null;
                if (firstShowtimeId.HasValue)
                {
                    showtime = await _context.Showtimes
                        .Include(s => s.Movie)
                        .FirstOrDefaultAsync(s => s.MaSuatChieu == firstShowtimeId.Value);
                }

                // Lấy danh sách đồ ăn
                var foods = await _context.InvoiceFoods
                    .Where(f => f.IdInvoice == invoice.IdInvoice)
                    .Join(_context.Foods,
                          f => f.IdFood,
                          food => food.IDFood,
                          (f, food) => new FoodItemDto
                          {
                              FoodName = food.FoodName,
                              Quantity = f.Quantity,
                              TotalPrice = f.TotalPrice
                          }).ToListAsync();

                result.Add(new InvoiceDetailDto
                {
                    IdInvoice = invoice.IdInvoice,
                    CreateAt = invoice.CreateAt,
                    TotalPrice = invoice.TotalPrice,
                    Status = invoice.Status,

                    MovieName = showtime?.Movie?.MovieName ?? "Không xác định",
                    NgayChieu = showtime?.NgayChieu ?? default,
                    GioChieu = showtime?.GioChieu ?? default,
                    Room = showtime != null ? $"Phòng {showtime.PhongChieu}" : "Không rõ",

                    Seats = bookedSeats.Select(b => b.SeatCode).ToList(),
                    Foods = foods
                });
            }

            return result;
        }
    }
}
