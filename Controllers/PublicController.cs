// GetMoviesController.cs
using API_Project.Models.DTOs;
using API_Project.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/Public")]
    public class PublicController : ControllerBase
    {
        private readonly PublicService _publicService;

        public PublicController(PublicService publicService)
        {
            _publicService = publicService;
        }

        //Cinema
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

        //Movie
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

        //Food
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
        [HttpGet("GetFoodById/{id}")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            try
            {
                var food = await _publicService.GetFoodByIdAsync(id);
                if (food == null)
                {
                    return NotFound(new { message = "Food not found" });
                }
                return Ok(food); // Đã sửa: Trả về dữ liệu food
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


        //Room
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
        [HttpGet("GetRoomById/{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            try
            {
                var room = await _publicService.GetRoomByIdAsync(id);
                if (room == null)
                {
                    return NotFound(new { message = "Room not found" });
                }
                return Ok(room);
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
