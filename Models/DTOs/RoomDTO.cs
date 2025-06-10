namespace API_Project.Models.DTOs
{
    public class RoomDTO
    {
        public int IDRoom { get; set; }
        public int CinemaID { get; set; }
        public string RoomName { get; set; }
        public int TotalSeats { get; set; }
        public int TicketPriceID { get; set; }
        public string ImageURL { get; set; }
    }
}
