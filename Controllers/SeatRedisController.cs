using API_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatRedisController : ControllerBase
    {
        private readonly RedisService _redisService;

        public SeatRedisController(RedisService redisService)
        {
            _redisService = redisService;
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
    }
}
