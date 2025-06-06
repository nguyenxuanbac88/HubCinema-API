// GetMoviesController.cs
using API_Project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/Public")]
    public class PublicController : ControllerBase
    {
        private readonly MovieService _movieService;

        public PublicController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("GetMovies")]
        public async Task<IActionResult> GetAllMovies()
        {
            try
            {
                var movies = await _movieService.GetAllMoviesAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
