namespace API_Project.Models.DTOs
{
    public class DashboardSummaryDto
    {
        public int TicketsToday { get; set; }
        public int TicketsThisMonth { get; set; }
        public int TicketsThisYear { get; set; }

        public int CurrentMovies { get; set; }
        public int UpcomingMovies { get; set; }

        public int ActiveCinemas { get; set; }
        public int TotalUsers { get; set; }
    }
    public class ChartDataPoint
    {
        public string Label { get; set; } = string.Empty;
        public int Value { get; set; }
    }

}
