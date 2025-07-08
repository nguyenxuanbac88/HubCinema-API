using API_Project.Models;
using API_Project.Models.DTOs;


namespace API_Project.Services
{
    public interface IScheduleService
    {
        Task<ApiResponse<FilterDataDto>> GetFilterDataAsync();
        Task<ApiResponse<List<DateTime>>> GetAvailableDatesAsync(int maPhim);
        Task<ApiResponse<List<GroupedShowtimeDTO>>> GetShowtimesAsync(int maPhim, DateTime date, string region = null, int? maRap = null);
        Task<ApiResponse<List<int>>> GetMovieIdsByCinemaAsync(int maRap);


    }

}
