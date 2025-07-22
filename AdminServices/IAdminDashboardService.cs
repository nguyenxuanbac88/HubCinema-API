using API_Project.Models.DTOs;

namespace API_Project.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
        Task<List<ChartDataPoint>> GetDailyTicketSalesAsync();
        Task<List<ChartDataPoint>> GetTicketSalesByCinemaAsync();
        Task<DashboardFullDto> GetFullDashboardAsync();
    }
}
