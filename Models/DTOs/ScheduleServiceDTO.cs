namespace API_Project.Models.DTOs
{
    public class CinemaDto
    {
        public int MaRap { get; set; }
        public string TenRap { get; set; }
        public string Region { get; set; }
    }
    public class FilterDataDto
    {
        public List<string> Regions { get; set; }
        public List<CinemaDto> Cinemas { get; set; }
    }
    public class GroupedShowtimeDTO
    {
        public int MaRap { get; set; }          // ID rạp
        public string TenRap { get; set; }      // Tên rạp
        public List<ShowtimeItemDTO> GioChieu { get; set; }
    }
}
