﻿using API_Project.Enums;
using API_Project.Models;
using API_Project.Data;
using Microsoft.EntityFrameworkCore;
using API_Project.Models.DTOs;
using API_Project.Models.Entities;

namespace API_Project.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _db;

        public ScheduleService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ApiResponse<FilterDataDto>> GetFilterDataAsync()
        {
            var regions = await _db.Cinemas
                .Select(r => r.City)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            regions.Insert(0, "Toàn quốc");

            var cinemas = await _db.Cinemas
                .Select(r => new CinemaDto
                {
                    MaRap = r.IDCinema,
                    TenRap = r.CinemaName,
                    Region = r.City
                }).ToListAsync();

            return ApiResponse<FilterDataDto>.Ok(new FilterDataDto
            {
                Regions = regions,
                Cinemas = cinemas
            });
        }

        public async Task<ApiResponse<List<DateTime>>> GetAvailableDatesAsync(int maPhim)
        {
            var exists = await _db.Movies.AnyAsync(p => p.IDMovie == maPhim);
            if (!exists)
                return ApiResponse<List<DateTime>>.Fail(ScheduleErrorCode.MovieNotFound, "Phim không tồn tại.");

            var dates = await _db.Showtimes
                .Where(s => s.MaPhim == maPhim)
                .Select(s => s.NgayChieu)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            if (!dates.Any())
                return ApiResponse<List<DateTime>>.Fail(ScheduleErrorCode.NoShowtimes, "Không có suất chiếu.");

            return ApiResponse<List<DateTime>>.Ok(dates);
        }

        public async Task<ApiResponse<List<GroupedShowtimeDTO>>> GetShowtimesAsync(int maPhim, DateTime date, string region = null, int? maRap = null)
        {
            if (!await _db.Movies.AnyAsync(p => p.IDMovie == maPhim))
                return ApiResponse<List<GroupedShowtimeDTO>>.Fail(ScheduleErrorCode.MovieNotFound, "Phim không tồn tại.");

            var query = _db.Showtimes
                .Include(s => s.Cinema)
                .Where(s => s.MaPhim == maPhim && s.NgayChieu == date);

            if (!string.IsNullOrEmpty(region) && region != "Toàn quốc")
                query = query.Where(s => s.Cinema.City == region);

            if (maRap.HasValue && maRap.Value != 0)
                query = query.Where(s => s.MaRap == maRap.Value);

            // Bước quan trọng: lấy toàn bộ ra memory để xử lý phức tạp
            var showtimeList = await query.ToListAsync();

            var groupedResult = showtimeList
                .GroupBy(s => new { s.MaRap, TenRap = s.Cinema?.CinemaName ?? "Không rõ" }) // fallback nếu Cinema null
                .Select(g => new GroupedShowtimeDTO
                {
                    MaRap = g.Key.MaRap,
                    TenRap = g.Key.TenRap,
                    GioChieu = g.Select(x => new ShowtimeItemDTO
                    {
                        GioChieu = x.GioChieu.ToString(@"hh\:mm"),
                        SuatChieu = x.MaSuatChieu
                    }).ToList()
                }).ToList();

            if (!groupedResult.Any())
                return ApiResponse<List<GroupedShowtimeDTO>>.Fail(ScheduleErrorCode.NoShowtimes, "Không tìm thấy suất chiếu phù hợp.");

            return ApiResponse<List<GroupedShowtimeDTO>>.Ok(groupedResult);
        }


        public async Task<ApiResponse<List<int>>> GetMovieIdsByCinemaAsync(int maRap)
        {
            var movieIds = await _db.Showtimes
                .Where(s => s.MaRap == maRap)
                .Select(s => s.MaPhim)
                .Distinct()
                .ToListAsync();

            if (movieIds == null || !movieIds.Any())
            {
                return ApiResponse<List<int>>.Fail(ScheduleErrorCode.NoShowtimes, "Không có suất chiếu.");
            }

            return ApiResponse<List<int>>.Ok(movieIds);
        }

        public async Task<bool> CreateShowtimeAsync(ShowtimeDTO showtimeDTO)
        {
            try
            {
                var movie = await _db.Movies.FirstOrDefaultAsync(m => m.IDMovie == showtimeDTO.MaPhim);
                if (movie == null)
                {
                    Console.WriteLine("ERROR: Phim không tồn tại.");
                    return false;
                }

                var newStart = showtimeDTO.GioChieu;
                var newEnd = newStart
                    .Add(TimeSpan.FromMinutes(movie.Duration + 20)); // 10' quảng cáo + 10' dọn rạp

                var showtimesInSameRoom = await _db.Showtimes
                    .Where(s => s.PhongChieu == showtimeDTO.PhongChieu && s.NgayChieu == showtimeDTO.NgayChieu)
                    .ToListAsync();

                foreach (var existing in showtimesInSameRoom)
                {
                    if (existing.GioKetThuc == null) continue;

                    bool isOverlap = newStart < existing.GioKetThuc && existing.GioChieu < newEnd;
                    if (isOverlap)
                    {
                        Console.WriteLine($"ERROR: Suất chiếu bị trùng tại phòng {showtimeDTO.PhongChieu}.");
                        return false;
                    }
                }

                var showTime = new Showtime
                {
                    MaSuatChieu = showtimeDTO.MaSuatChieu,
                    PhongChieu = showtimeDTO.PhongChieu,
                    MaPhim = showtimeDTO.MaPhim,
                    NgayChieu = showtimeDTO.NgayChieu,
                    GioChieu = newStart,
                    GioKetThuc = newEnd,
                    ChiPhi = showtimeDTO.ChiPhi,
                    TypeSuatChieu = showtimeDTO.TypeSuatChieu,
                    MaRap = showtimeDTO.MaRap
                };

                _db.Showtimes.Add(showTime);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                return false;
            }
        }
        public async Task<List<ShowtimeTimelineDTO>> GetShowtimesTimelineByDateAndCinemaAsync(DateTime ngay, int maRap)
        {
            var showtimes = await _db.Showtimes
                .Include(s => s.Movie)
                .Where(s => s.NgayChieu.Date == ngay.Date && s.MaRap == maRap)
                .Select(s => new ShowtimeTimelineDTO
                {
                    Id = s.MaSuatChieu.ToString(),
                    Name = s.Movie.MovieName,
                    Start = s.NgayChieu.Date.Add(s.GioChieu).ToString("yyyy-MM-ddTHH:mm:ss"),
                    End = s.NgayChieu.Date.Add(s.GioKetThuc ?? s.GioChieu.Add(TimeSpan.FromMinutes(120))).ToString("yyyy-MM-ddTHH:mm:ss"),
                    Resource = $"Phòng {s.PhongChieu}"
                })
                .ToListAsync();

            return showtimes;
        }

    }
}
