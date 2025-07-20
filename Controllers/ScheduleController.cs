using API_Project.AdminServices;
using API_Project.Models.DTOs;
using API_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("filter-data")]
        public async Task<IActionResult> GetFilterData()
        {
            var result = await _scheduleService.GetFilterDataAsync();
            return Ok(result);
        }

        [HttpGet("dates")]
        public async Task<IActionResult> GetDates([FromQuery] int maPhim)
        {
            var result = await _scheduleService.GetAvailableDatesAsync(maPhim);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetShowtimes([FromQuery] int maPhim, [FromQuery] DateTime date, [FromQuery] string region = null, [FromQuery] int? maRap = null)
        {
            var result = await _scheduleService.GetShowtimesAsync(maPhim, date, region, maRap);
            return Ok(result);
        }

        [HttpGet("GetMovieIdsByCinema")]
        public async Task<IActionResult> GetMovieIdsByCinema([FromQuery] int maRap)
        {
            var result = await _scheduleService.GetMovieIdsByCinemaAsync(maRap);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("CreateSchedule")]
        public async Task<IActionResult> CreateSchedule([FromBody] ShowtimeDTO showtimeDTO)
        {
            if (showtimeDTO == null)
                return BadRequest(new { Success = false, Message = "Dữ liệu suất chiếu không được để trống." });

            try
            {
                var result = await _scheduleService.CreateShowtimeAsync(showtimeDTO);

                if (result)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Tạo suất chiếu thành công."
                    });
                }

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Tạo suất chiếu thất bại."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Lỗi hệ thống.",
                    Error = ex.Message
                });
            }
        }
        [HttpGet("GetTimeline")]
        public async Task<IActionResult> GetTimeline([FromQuery] DateTime ngay, [FromQuery] int maRap)
        {
            try
            {
                var result = await _scheduleService.GetShowtimesTimelineByDateAndCinemaAsync(ngay, maRap);

                if (result == null || !result.Any())
                    return NotFound(new { Success = false, Message = "Không có suất chiếu nào trong ngày." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Đã xảy ra lỗi.", Error = ex.Message });
            }
        }

    }
}
