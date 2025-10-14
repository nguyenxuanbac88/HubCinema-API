namespace API_Project.Models.Entities
{
    public class TicketBookingResultDto
    {
        public int IdInvoice { get; set; }
        public int TotalPrice { get; set; }
        public int PointUsed { get; set; }
        public int PointEarned { get; set; }
        public List<SeatInfoDto> Seats { get; set; }
        public List<FoodInfoDto> Foods { get; set; }
    }

    public class SeatInfoDto
    {
        public string SeatCode { get; set; }
        public int Price { get; set; }
    }

    public class FoodInfoDto
    {
        public int IdFood { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }

}
