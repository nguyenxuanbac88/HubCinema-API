using API_Project.AdminServices;
using API_Project.Models.DTOs;
using API_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
