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
        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            var created = await _newsService.CreateAsync(news);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Cập nhật bài viết.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, News news)
        {
            if (id != news.Id) return BadRequest();
            var result = await _newsService.UpdateAsync(news);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Xoá bài viết.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _newsService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        // ------------------ CATEGORY API GỘP VÀO ĐÂY ------------------

        /// <summary>
        /// Lấy tất cả danh mục đang hiển thị.
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _newsService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Lấy danh mục theo ID.
        /// </summary>
        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _newsService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
    }
}
