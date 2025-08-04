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
        public async Task<List<object>> Invoice(int userId)
        {
            var invoices = await _context.Invoices
                .Where(i => i.IdUser == userId)
                .ToListAsync();

            var result = new List<object>();

            foreach (var invoice in invoices)
            {
                var bookedSeats = await _context.BookedSeats
                    .Where(b => b.InvoiceId == invoice.IdInvoice)
                    .ToListAsync();

                if (bookedSeats == null || !bookedSeats.Any())
                    continue;

                var firstShowtimeId = bookedSeats.FirstOrDefault()?.ShowtimeId;

                Showtime? showtime = null;
                if (firstShowtimeId.HasValue)
                {
                    showtime = await _context.Showtimes
                        .Include(s => s.Movie)
                        .Include(s => s.Cinema)
                        .FirstOrDefaultAsync(s => s.MaSuatChieu == firstShowtimeId.Value);
                }

                // Lấy danh sách đồ ăn
                var invoiceFoods = await _context.InvoiceFoods
                    .Where(f => f.IdInvoice == invoice.IdInvoice)
                    .Join(_context.Foods,
                          f => f.IdFood,
                          food => food.IDFood,
                          (f, food) => new
                          {
                              foodName = food.FoodName,
                              quantity = f.Quantity,
                              price = f.TotalPrice
                          }).ToListAsync();

                var comboTotal = invoiceFoods.Sum(f => f.price);

                result.Add(new
                {
                    orderId = invoice.IdInvoice,
                    movieTitle = showtime?.Movie?.MovieName ?? "Không xác định",
                    posterUrl = showtime?.Movie?.CoverURL ?? "",
                    cinemaName = showtime?.Cinema?.CinemaName ?? "Không xác định",
                    roomName = showtime != null ? $"Phòng {showtime.PhongChieu}" : "Không rõ",
                    showTime = showtime != null
                        ? showtime.NgayChieu.Date + showtime.GioChieu
                        : DateTime.MinValue,
                    seats = string.Join(", ", bookedSeats.Select(b => b.SeatCode)),
                    price = invoice.TotalPrice,
                    comboTotal = comboTotal,
                    foods = invoiceFoods
                });
            }

            return result;
        }
        public async Task<object?> GetInvoiceById(int invoiceId)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.IdInvoice == invoiceId);

            if (invoice == null)
                return null;

            var bookedSeats = await _context.BookedSeats
                .Where(b => b.InvoiceId == invoice.IdInvoice)
                .ToListAsync();

            if (!bookedSeats.Any())
                return null;

            var firstShowtimeId = bookedSeats.FirstOrDefault()?.ShowtimeId;

            Showtime? showtime = null;
            if (firstShowtimeId.HasValue)
            {
                showtime = await _context.Showtimes
                    .Include(s => s.Movie)
                    .Include(s => s.Cinema)
                    .FirstOrDefaultAsync(s => s.MaSuatChieu == firstShowtimeId.Value);
            }

            var invoiceFoods = await _context.InvoiceFoods
                .Where(f => f.IdInvoice == invoice.IdInvoice)
                .Join(_context.Foods,
                      f => f.IdFood,
                      food => food.IDFood,
                      (f, food) => new
                      {
                          foodName = food.FoodName,
                          quantity = f.Quantity,
                          price = f.TotalPrice
                      }).ToListAsync();

            var comboTotal = invoiceFoods.Sum(f => f.price);

            return new
            {
                orderId = invoice.IdInvoice,
                movieTitle = showtime?.Movie?.MovieName ?? "Không xác định",
                posterUrl = showtime?.Movie?.CoverURL ?? "",
                cinemaName = showtime?.Cinema?.CinemaName ?? "Không xác định",
                roomName = showtime != null ? $"Phòng {showtime.PhongChieu}" : "Không rõ",
                showTime = showtime != null
                    ? showtime.NgayChieu.Date + showtime.GioChieu
                    : DateTime.MinValue,
                seats = string.Join(", ", bookedSeats.Select(b => b.SeatCode)),
                price = invoice.TotalPrice,
                comboTotal = comboTotal,
                foods = invoiceFoods
            };
        }

    }
}
