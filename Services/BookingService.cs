using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using API_Project.Models;
using API_Project.Models.DTOs;
using API_Project.Helpers;
using API_Project.Services.Interfaces;
using API_Project.Data;
using API_Project.Models.Entities;
using Entities = API_Project.Models.Entities;
using API_Project.Enums;

namespace API_Project.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _db;
        private readonly ISeatLayoutCacheService _seatLayoutService;

        public BookingService(ApplicationDbContext db, ISeatLayoutCacheService seatLayoutService)
        {
            _db = db;
            _seatLayoutService = seatLayoutService;
        }

        public async Task<ApiResponse<int>> BookTicketsAsync(int userId, TicketBookingRequestDto request)
        {
            // 1. Lấy layout ghế từ Redis
            //var layout = await _seatLayoutService.GetSeatLayoutAsync(request.IdShowtime);
            //if (layout == null)
            //    return ApiResponse<int>.Fail(ScheduleErrorCode.LayoutNotFound, "Layout ghế không tồn tại hoặc đã hết hạn");

            // 2. Kiểm tra từng ghế
            //foreach (var seat in request.Seats)
            //{
            //    if (!layout.ContainsKey(seat.MaGhe))
            //        return ApiResponse<int>.Fail(ScheduleErrorCode.InvalidSeat, $"Ghế {seat.MaGhe} không hợp lệ");

            //    if (layout[seat.MaGhe].Price != seat.Price)
            //        return ApiResponse<int>.Fail(ScheduleErrorCode.PriceMismatch, $"Giá ghế {seat.MaGhe} không khớp với hệ thống");
            //}
            // 1. Tính tổng tiền
            int tongTienGhe = request.Seats.Sum(g => g.Price);
            int tongTienDoAn = request.Foods.Sum(d => d.Price * d.Quantity);
            int tongTien = tongTienGhe + tongTienDoAn;

            // 2. Tạo hóa đơn
            var invoice = new Invoice
            {
                IdUser = userId,
                CreateAt = DateTime.Now,
                TotalPrice = tongTien,
                IdVoucher = request.IdVoucher,
                PointUsed = request.UsedPoint,
                PointEarned = (int)(tongTien * 0.05),
                Status = 1 // Đã thanh toán
            };

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync(); // Lưu trước để có IdInvoice

            Console.WriteLine($"DEBUG - Invoice Id: {invoice.IdInvoice}");

            // 3. Lưu ghế đã đặt
            foreach (var seat in request.Seats)
            {
                var booked = new BookedSeat
                {
                    ShowtimeId = request.IdShowtime,
                    SeatCode = seat.MaGhe,
                    UserId = userId,
                    Price = seat.Price,
                    Status = BookingStatuses.ChoThanhToan,
                    PurchaseMethod = "Online",
                    CreatedAt = DateTime.Now,
                    InvoiceId = invoice.IdInvoice
                };

                _db.BookedSeats.Add(booked);
            }

            // 4. Lưu đồ ăn kèm theo hóa đơn
            if (request.Foods != null && request.Foods.Count > 0)
            {
                foreach (var item in request.Foods)
                {
                    _db.InvoiceFoods.Add(new InvoiceFood
                    {
                        IdInvoice = invoice.IdInvoice,
                        IdFood = item.IdFood,
                        Quantity = item.Quantity,
                        TotalPrice = item.Price * item.Quantity
                    });
                }
            }

            try
            {
                await _db.SaveChangesAsync(); // Lưu toàn bộ ghế + đồ ăn
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lưu BookedSeats hoặc InvoiceFoods: " + ex.Message);
                return ApiResponse<int>.Fail(ScheduleErrorCode.SaveError, "Lỗi khi lưu dữ liệu đặt vé.");
            }

            return ApiResponse<int>.Ok(invoice.IdInvoice);
        }
    }
}
