using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("Phim")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MaPhim")]
        public int? IDMovie { get; set; }
       
        [Column("TenPhim")]
        [MaxLength(255)]
        public string MovieName { get; set; }

        [Column("TheLoai")]
        [MaxLength(50)]
        public string Genre { get; set; }

        [Column("ThoiLuong")]
        public int Duration { get; set; }

        [Column("MoTa")]
        public string Description { get; set; }

        [Column("DaoDien")]
        [MaxLength(50)]
        public string Director { get; set; }

        [Column("NgayKhoiChieu")]
        public DateTime ReleaseDate { get; set; }

        [Column("BiaURL")]
        public string CoverURL { get; set; }

        [Column("TrailerURL")]
        public string TrailerURL { get; set; }

        [Column("GioiHanTuoi")]
        public string AgeRestriction { get; set; }

        [Column("NhaSanXuat")]
        public string Producer { get; set; }

        [Column("DienVien")]
        public string Actors { get; set; }
        public ICollection<Showtime> Showtimes { get; set; }

    }
}
