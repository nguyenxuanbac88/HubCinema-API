using API_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly SeatLayoutService _seatLayoutService;
        public SeatController(RedisService redisService, SeatLayoutService seatLayoutService)
        {
            _redisService = redisService;
            _seatLayoutService = seatLayoutService;
        }

        // POST: api/Seat/hold
        [HttpPost("hold")]
        public async Task<IActionResult> HoldSeat(int showtimeId, string seatCode, string userId)
        {
            var success = await _redisService.HoldSeatAsync(showtimeId, seatCode, userId);
            return success ? Ok("Seat held successfully") : BadRequest("Seat is already held by someone else");
        }

        // GET: api/Seat/check?showtimeId=101&seatCode=A5
        [HttpGet("check")]
        public async Task<IActionResult> CheckSeatStatus(int showtimeId, string seatCode)
        {
            var isHeld = await _redisService.IsSeatLockedAsync(showtimeId, seatCode);
            return Ok(new { Seat = seatCode, IsHeld = isHeld });
        }

        // DELETE: api/Seat/release?showtimeId=101&seatCode=A5
        [HttpDelete("release")]
        public async Task<IActionResult> ReleaseSeat(int showtimeId, string seatCode)
        {
            var removed = await _redisService.ReleaseSeatAsync(showtimeId, seatCode);
            return removed ? Ok("Seat released") : NotFound("Seat was not held");
        }

        // GET: api/Seat/held-list/101
        [HttpGet("held-list/{showtimeId}")]
        public async Task<IActionResult> GetHeldSeats(int showtimeId)
        {
            var list = await _redisService.GetHeldSeatsAsync(showtimeId);
            return Ok(list);
        }

        [HttpGet("get-layout-price/{idSuatChieu}")]
        public async Task<IActionResult> GetLayoutWithPrices(int idSuatChieu)
        {
            var result = await _seatLayoutService.GetFullSeatLayoutWithPricesAsync(idSuatChieu);
            return Ok(result);
        }
    }
}
