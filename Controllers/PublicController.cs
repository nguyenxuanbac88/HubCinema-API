// GetMoviesController.cs
using API_Project.Models.DTOs;
using API_Project.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/Public")]
    public class PublicController : ControllerBase
    {
        private readonly PublicService _publicService;

        public PublicController(PublicService movieService)
        {
            _publicService = movieService;
        }

        [HttpGet("GetMovies")]
        public async Task<IActionResult> GetAllMovies()
        {
            try
            {
                var movies = await _publicService.GetAllMoviesAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    
        [HttpGet("GetFoods")]
        public async Task<IActionResult> GetAllFoods()
        {
            try
            {
                var foods = await _publicService.GetAllFoodsAsync();
                return Ok(foods);
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
        [HttpGet("GetCinemas")]
        public async Task<IActionResult> GetAllCinemas()
        {
            try
            {
                var cinemas = await _publicService.GetAllCinemaAsync();
                return Ok(cinemas);
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
        [HttpGet("GetRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                var rooms = await _publicService.GetAllRoomAsync();
                return Ok(rooms);
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
        [HttpGet("GetMovieById/{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            try
            {
                var movie = await _publicService.GetMovieByIdAsync(id);
                if (movie == null)
                {
                    return NotFound(new { message = "Movie not found" });
                }
                return Ok(movie);
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
        [HttpGet("GetCinemaById/{id}")]
        public async Task<IActionResult> GetCinemaById(int id)
        {
            try
            {
                var cinema = await _publicService.GetCinemaByIdAsync(id);
                if (cinema == null)
                {
                    return NotFound(new { message = "Cinema not found" });
                }
                return Ok(cinema);
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
