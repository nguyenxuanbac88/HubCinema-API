using API_Project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/AdminPUT")]
    public class AdminController : Controller
    {
        private readonly PublicService _publicService;
        public AdminController(PublicService publicService)
        {
            _publicService = publicService;
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
                var result = await _publicService.UpdateMovieAsync(id, movieDTO);
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
                var result = await _publicService.UpdateCinema(id, cinemaDTO);
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
                var result = await _publicService.CreateMovie(movieDTO);
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
                var result = await _publicService.CreateCinema(cinemaDTO);
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
    }
}
