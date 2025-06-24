using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("NguoiDung")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MaNguoiDung")]
        public int IDUser { get; set; }

        [MaxLength(30)]
        [Column("MaBarcode")]
        public string? MaBarcode { get; set; }

        [Column("SoDienThoai")]
        [MaxLength(10)]
        public string Phone { get; set; }

        [Column("MatKhau")]
        [MaxLength(50)]
        public string Password { get; set; }

        [Column("Email")]
        [MaxLength(80)]
        public string Email { get; set; }

        [Column("HoTen")]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Column("NgaySinh")]
        public DateTime Dob { get; set; }

        [Column("GioiTinh")]
        public bool Gender { get; set; }

        [Column("VaiTro")]
        public byte Role { get; set; }

        [Column("TokenMatKhau")]
        [MaxLength(255)]
        public string? TokenPassword { get; set; }

        [Column("Diem")]
        public int? Points { get; set; }

        [Column("TongChiTieu")]
        public int? TotalSpending { get; set; }
        [Column("ThuHang")]
        public int? Rank { get; set; }

        [Column("TokenNganHan")]
        public string? TokenLogin { get; set; }

        [Column("TokenDaiHan")]
        public string? TokenDaiHan { get; set; }

        [Column("KhuVuc")]
        public string? ZoneAddress { get; set; }
        [Column("Otp")]
        public string? OTP { get; set; }

        [Column("TimeOtp")]
        public long? TimeOtp { get; set; }
        [Column("EmailPending")]
        public string? EmailPending { get; set; }

    }
}
