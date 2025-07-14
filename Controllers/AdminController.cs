using API_Project.AdminServices;
using API_Project.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Authorize(Roles = "Admin")]
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
                    return Ok(new { message = "Cinema created successfully" });
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
        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoom([FromBody] RoomDTO roomDTO)
        {
            if (roomDTO == null)
            {
                return BadRequest("Room data is null");
            }
            try
            {
                var result = await _privateService.CreateRoom(roomDTO);
                if (result)
                {
                    return Ok(new { message = "Room created successfully" });
                }
                else
                {
                    return StatusCode(500, "Failed to create Room");
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
        [HttpPost("CreateFood")]
        public async Task<IActionResult> CreateFood([FromBody] FoodDTO foodDTO)
        {
            if (foodDTO == null)
                return BadRequest("Food data is null");

            try
            {
                var createdFood = await _privateService.CreateFood(foodDTO);

                if (createdFood != null && createdFood.IDFood != null)
                {
                    return Ok(createdFood);
                }

                return StatusCode(500, new
                {
                    message = "Failed to create food",
                    error = "Food creation returned null"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }


        [HttpPost("CreateComboForCinemas")]
        public async Task<IActionResult> CreateComboForCinemas([FromBody] CreateComboCinema CreateComboCinema)
        {
            if (CreateComboCinema == null)
            {
                return BadRequest("Combo data is null");
            }
            try
            {
                var result = await _privateService.CreateComboForCinemasAsync(CreateComboCinema);
                if (result)
                {
                    return Ok(new { message = "Combo created successfully" });
                }
                else
                {
                    return StatusCode(500, "Failed to create Combo");
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
        [HttpGet("GetRoomsByCinemaId/{idCinema}")]
        public async Task<IActionResult> GetRoomsByCinemaId(int idCinema)
        {
            var rooms = await _privateService.GetRoomsByCinemaAsync(idCinema);

            if (rooms == null || rooms.Count == 0)
            {
                return NotFound($"Không tìm thấy phòng chiếu cho rạp có tên: {idCinema}");
            }

            return Ok(rooms);
        }
    }
}
