using API_Project.Models.DTOs;

namespace API_Project.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    }
}
