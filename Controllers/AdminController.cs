using API_Project.Models.DTOs;
using API_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        private readonly PrivateService _privateService;
        public AdminController(PublicService publicService, PrivateService privateService)
        {
            _privateService = privateService;
        }
        [HttpPut("UpdateMovie/{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDTO movieDTO)
        {
            if (movieDTO == null)
            {
                return BadRequest("Movie data is null");
            }
            try
            {
                var result = await _privateService.UpdateMovieAsync(id, movieDTO);
                if (result)
                {
                    return Ok(new { message = "Movie updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Movie not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        [HttpPut("UpdateCinema/{id}")]
        public async Task<IActionResult> UpdateCinema(int id, [FromBody] CinemaDTO cinemaDTO)
        {
            if (cinemaDTO == null)
            {
                return BadRequest("Cinema data is null");
            }
            try
            {
                var result = await _privateService.UpdateCinema(id, cinemaDTO);
                if (result)
                {
                    return Ok(new { message = "Cinema updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Cinema not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        [HttpPost("CreateMovie")]
        public async Task<IActionResult> CreateMovie([FromBody] MovieDTO movieDTO)
        {
            if (movieDTO == null)
            {
                return BadRequest("Movie data is null");
            }
            try
            {
                var result = await _privateService.CreateMovie(movieDTO);
                if (result)
                {
                    return Ok(new { message = "Movie created successfully" });
                }
                else
                {
                    return StatusCode(500, "Failed to create movie");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        [HttpPost("CreateCinema")]
        public async Task<IActionResult> CreateCinema([FromBody] CinemaDTO cinemaDTO)
        {
            if (cinemaDTO == null)
            {
                return BadRequest("Movie data is null");
            }
            try
            {
                var result = await _privateService.CreateCinema(cinemaDTO);
                if (result)
                {
                    return Ok(new { message = "Movie created successfully" });
                }
                else
                {
                    return StatusCode(500, "Failed to create movie");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        [HttpGet("GetRoomsByCinemaName/{nameCinema}")]
        public async Task<IActionResult> GetRoomsByCinemaName(string nameCinema)
        {
            var rooms = await _privateService.GetRoomsByCinemaAsync(nameCinema);

            if (rooms == null || rooms.Count == 0)
            {
                return NotFound($"Không tìm thấy phòng chiếu cho rạp có tên: {nameCinema}");
            }

            return Ok(rooms);
        }
    }
}
