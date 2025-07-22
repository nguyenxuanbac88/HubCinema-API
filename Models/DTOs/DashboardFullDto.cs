namespace API_Project.Models.DTOs
{
    public class DashboardFullDto
    {
        public DashboardSummaryDto Summary { get; set; }
        public List<ChartDataPoint> DailySales { get; set; }
        public List<ChartDataPoint> CinemaSales { get; set; }
    }

}
