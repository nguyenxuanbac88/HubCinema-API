using API_Project.AdminServices;
using API_Project.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatTypeInRoomController : ControllerBase
    {
        private readonly ISeatTypeInRoomService _service;

        public SeatTypeInRoomController(ISeatTypeInRoomService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SeatTypeInRoomRequest request)
        {
            var success = await _service.CreateAsync(request);
            if (!success) return BadRequest("Thêm thất bại.");
            return Ok("Thêm thành công.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SeatTypeInRoomRequest request)
        {
            var success = await _service.UpdateAsync(id, request);
            if (!success) return NotFound("Không tìm thấy bản ghi.");
            return Ok("Cập nhật thành công.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound("Không tìm thấy bản ghi.");
            return Ok("Xoá thành công.");
        }
    }

}
