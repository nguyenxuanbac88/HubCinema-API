using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("HoaDon")]
    public class Invoice
    {
        [Key]
        [Column("MaHoaDon")]
        public int IdInvoice { get; set; }

        [Column("MaNguoiDung")]
        public int IdUser { get; set; }

        [Column("ThoiGianTao")]
        public DateTime CreateAt { get; set; }

        [Column("TongTien")]
        public int TotalPrice { get; set; }

        [Column("MaVoucher")]
        public int? IdVoucher { get; set; }

        [Column("TruTienDiem")]
        public int PointUsed { get; set; }

        [Column("TichDiem")]
        public int PointEarned { get; set; }

        [Column("TrangThai")]
        public byte Status { get; set; }

    }
}
