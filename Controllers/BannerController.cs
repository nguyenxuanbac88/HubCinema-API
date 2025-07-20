using API_Project.Models.Entities;
using API_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly BannerService _bannerService;

        public BannerController(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        /// <summary>
        /// Lấy danh sách tất cả banner đang hiển thị (IsActive = true).
        /// </summary>
        /// <returns>Danh sách banner đang hoạt động</returns>
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveBanners()
        {
            var banners = await _bannerService.GetActiveBannersAsync();
            return Ok(banners);
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một banner theo ID.
        /// </summary>
        /// <param name="id">ID banner</param>
        /// <returns>Thông tin banner</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBannerById(int id)
        {
            var banner = await _bannerService.GetByIdAsync(id);
            if (banner == null)
                return NotFound();

            return Ok(banner);
        }

        /// <summary>
        /// Tạo mới một banner.
        /// </summary>
        /// <param name="banner">Thông tin banner</param>
        /// <returns>Banner vừa tạo</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBanner([FromBody] Banner banner)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _bannerService.AddBannerAsync(banner);
            return success ? Ok(banner) : StatusCode(500, "Thêm banner thất bại.");
        }

        /// <summary>
        /// Cập nhật thông tin một banner theo ID.
        /// </summary>
        /// <param name="id">ID banner</param>
        /// <param name="banner">Thông tin cập nhật</param>
        /// <returns>Trạng thái cập nhật</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBanner(int id, [FromBody] Banner banner)
        {
            if (id != banner.BannerId)
                return BadRequest("ID không khớp.");

            var success = await _bannerService.UpdateBannerAsync(banner);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Xoá một banner theo ID.
        /// </summary>
        /// <param name="id">ID banner</param>
        /// <returns>Trạng thái xoá</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var success = await _bannerService.DeleteBannerAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
