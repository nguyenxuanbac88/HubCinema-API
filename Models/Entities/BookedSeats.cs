using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("GheDaDat")]
    public class BookedSeat
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("SuatChieuId")]
        public int ShowtimeId { get; set; }

        [Column("MaGhe")]
        public string SeatCode { get; set; }

        [Column("MaNguoiDung")]
        public int UserId { get; set; }

        [Column("GiaVe")]
        public int Price { get; set; }

        [Column("TrangThai")]
        public string Status { get; set; }

        [Column("HinhThucmua")]
        public string PurchaseMethod { get; set; }

        [Column("ThoiGianDat")]
        public DateTime CreatedAt { get; set; }

        [Column("MaHoaDon")]
        public int InvoiceId { get; set; }
    }
}
