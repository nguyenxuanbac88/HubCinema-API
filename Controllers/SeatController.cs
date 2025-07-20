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
        public class HoldSeatRequest
        {
            public int ShowtimeId { get; set; }
            public List<string> SeatCodes { get; set; }
        }

        [HttpPost("hold-multiple")]
        public async Task<IActionResult> HoldMultipleSeats([FromBody] HoldSeatRequest request)
        {
            var results = new Dictionary<string, bool>();

            foreach (var seat in request.SeatCodes)
            {
                var success = await _redisService.HoldSeatAsync(request.ShowtimeId, seat);
                results[seat] = success;
            }

            return Ok(results);
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

        // GET: api/Seat/get-layout-price/{idSuatChieu}
        [HttpGet("get-layout-price/{idSuatChieu}")]
        public async Task<IActionResult> GetLayoutWithPrices(int idSuatChieu)
        {
            var result = await _seatLayoutService.GetFullSeatLayoutWithPricesAsync(idSuatChieu);
            return Ok(result);
        }
    }
}
