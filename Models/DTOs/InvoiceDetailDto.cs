namespace API_Project.Models.DTOs
{
    public class InvoiceDetailDto
    {
        public int IdInvoice { get; set; }
        public DateTime CreateAt { get; set; }
        public int TotalPrice { get; set; }
        public byte Status { get; set; }

        public string MovieName { get; set; }
        public string posterUrl { get; set; }
        public DateTime NgayChieu { get; set; }
        public TimeSpan GioChieu { get; set; }
        public string Room { get; set; }

        public List<string> Seats { get; set; }
        public List<FoodItemDto> Foods { get; set; }
        public int IdUser { get; internal set; }
        public string? CinemaName { get; internal set; }
    }
    public class InvoiceDetail1Dto
    {
        public string OrderId { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public string CinemaName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime ShowTime { get; set; }
        public string Seats { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal ComboTotal { get; set; }

        public List<object> Foods { get; set; } = new();
    }

    public class FoodItemDto
    {
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }

}
