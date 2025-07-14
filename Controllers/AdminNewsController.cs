using API_Project.Models.Entities;
using API_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Lấy danh sách tất cả bài viết (News).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _newsService.GetAllAsync();
            return Ok(list);
        }

        /// <summary>
        /// Lấy chi tiết bài viết theo ID.
        /// </summary>
        /// <param name="id">ID của bài viết cần lấy</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _newsService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Tạo mới một bài viết.
        /// </summary>
        /// <param name="news">Đối tượng bài viết cần tạo</param>
        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            var created = await _newsService.CreateAsync(news);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Cập nhật thông tin bài viết theo ID.
        /// </summary>
        /// <param name="id">ID của bài viết</param>
        /// <param name="news">Thông tin mới của bài viết</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, News news)
        {
            if (id != news.Id) return BadRequest();
            var result = await _newsService.UpdateAsync(news);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Xoá bài viết theo ID.
        /// </summary>
        /// <param name="id">ID của bài viết cần xoá</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _newsService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
