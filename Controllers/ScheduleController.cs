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
    }
}
