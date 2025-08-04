using API_Project.AdminServices;
using API_Project.Models.DTOs;
using API_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// Lấy tất cả hóa đơn kèm chi tiết ghế, đồ ăn, phim
        /// </summary>
        /// <returns>Danh sách hóa đơn chi tiết</returns>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<InvoiceDetailDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllInvoices()
        {
            var result = await _invoiceService.GetAllInvoicesAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("by-user")]
        [ProducesResponseType(typeof(List<InvoiceDetailDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInvoicesByToken()
        {
            // Lấy userId từ JWT token
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
            {
                return Unauthorized("Không tìm thấy thông tin người dùng trong token.");
            }

            var userId = int.Parse(userIdClaim.Value);

            var result = await _invoiceService.Invoice(userId);
            return Ok(result);
        }
        [HttpGet("{invoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoiceById(int invoiceId)
        {
            var result = await _invoiceService.GetInvoiceById(invoiceId);

            if (result == null)
                return NotFound($"Không tìm thấy hóa đơn với Id: {invoiceId}");

            return Ok(result);
        }

    }
}
