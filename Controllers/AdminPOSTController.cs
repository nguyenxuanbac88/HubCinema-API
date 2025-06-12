using API_Project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/AdminPOST")]
    public class AdminPOSTController : Controller
    {
        private readonly PublicService _publicService;
        public AdminPOSTController(PublicService publicService)
        {
            _publicService = publicService;
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
    }
}
