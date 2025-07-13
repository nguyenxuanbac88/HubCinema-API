using API_Project.Models.DTOs;
using API_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("book")]
    [Authorize]
    public async Task<IActionResult> BookTickets([FromBody] TicketBookingRequestDto request)
    {
        var userIdClaim = User.FindFirst("id");
        if (userIdClaim == null)
            return Unauthorized("Người dùng chưa đăng nhập");

        if (!int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token không hợp lệ");

        var result = await _bookingService.BookTicketsAsync(userId, request);

        if (!result.Success)
            return BadRequest(new { message = result.Message });

        return Ok(new
        {
            message = "Đặt vé thành công",
            invoiceId = result.Data
        });
    }
}
