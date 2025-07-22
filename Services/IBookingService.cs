using API_Project.Models.DTOs;
using API_Project.Helpers;
using System.Threading.Tasks;
using API_Project.Models;
using API_Project.Models.Entities;

namespace API_Project.Services.Interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse<int>> BookTicketsAsync(int userId, TicketBookingRequestDto request);
        Task<ApiResponse<bool>> UpdateSeatStatusToPaidAsync(int invoiceId);
        //Task<ApiResponse<List<Invoice>>> GetInvoicesByUserIdAsync(int userId);
    }
}
