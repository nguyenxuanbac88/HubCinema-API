using API_Project.AdminServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")] // nếu có xác thực
    public class AdminUserController : ControllerBase
    {
        private readonly IUserAdminService _userService;
        private readonly IInvoiceService _invoiceService;

        public AdminUserController(IUserAdminService userService, IInvoiceService invoiceService)
        {
            _userService = userService;
            _invoiceService = invoiceService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? keyword)
        {
            var users = await _userService.GetAllUsersAsync(keyword);
            return Ok(users);
        }

        [HttpGet("{userId}/invoices")]
        public async Task<IActionResult> GetUserInvoices(int userId)
        {
            var invoices = await _invoiceService.Invoice(userId);
            return Ok(invoices);
        }
    }
}
