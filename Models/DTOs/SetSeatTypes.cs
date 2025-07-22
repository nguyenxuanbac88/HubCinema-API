namespace API_Project.Models.DTOs
{
    public class SetSeatTypes
    {
        public int MaPhong { get; set; }
        public int MaRap { get; set; }
        public List<SeatTypeConfigDto> DanhSachGhe { get; set; } = new();
    }
}
