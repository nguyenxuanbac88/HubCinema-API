namespace API_Project.Models.DTOs
{
    public class TicketBookingRequestDto
    {
        public int IdShowtime { get; set; }                         // khớp với BookedSeats.IdShowtime
        public List<SeatRequestDto> Seats { get; set; }            // danh sách ghế
        public List<FoodRequestDto> Foods { get; set; }           // danh sách đồ ăn
        public int? IdVoucher { get; set; }                         // mã voucher (nullable)
        public int UsedPoint { get; set; }                          // điểm sử dụng
    }

    public class SeatRequestDto
    { 
        public int Price { get; set; }                              // giá vé ghế đó
        public string MaGhe { get; set; }
    }

    public class FoodRequestDto
    {
        public int IdFood { get; set; }                             // mã đồ ăn -> khớp với InvoiceFood.IdFood
        public int Quantity { get; set; }                           // số lượng
        public int Price { get; set; }                              // giá đơn vị
    }
}
