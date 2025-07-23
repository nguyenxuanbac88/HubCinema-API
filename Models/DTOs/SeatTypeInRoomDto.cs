namespace API_Project.Models.DTOs
{
    public class SeatTypeInRoomDto
    {
        public int Id { get; set; }
        public string CinemaName { get; set; }
        public string RoomName { get; set; }
        public string RowCode { get; set; }
        public string SeatType { get; set; }
        public long Price { get; set; }
    }
    public class SeatTypeInRoomRequest
    {
        public int CinemaId { get; set; }
        public int RoomId { get; set; }
        public string RowCode { get; set; }
        public string SeatType { get; set; }
        public long Price { get; set; }
    }

}
