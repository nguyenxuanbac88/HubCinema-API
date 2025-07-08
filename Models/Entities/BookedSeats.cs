using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("GheDaDat")]
    public class BookedSeats
    {
        [Key]
        [Column("id")]
        public int IDBooked { get; set; }
        [Column("SuatChieuId")]
        public int IdShowtime { get; set; }
        [Column("MaGhe")]
        public string IdSeat { get; set; }
        [Column("MaNguoiDung")]
        public int IdUser { get; set; }
        [Column("GiaVe")]
        public int Price { get; set; }
        [Column("TrangThai")]
        public string status { get; set; }
        [Column("HinhThucmua")]
        public string purchase { get; set; }
        [Column("ThoiGianDat")]
        public DateTime CreateAt { get; set; }

    }
}
