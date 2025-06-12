using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_Project.Models.Entities
{
    [Table("PhongChieu")]
    public class Room
    {
        [Key]
        [Column("MaPhong")]
        public int IDRoom { get; set; }

        [Column("MaRap")]
        public int CinemaID { get; set; }

        [Column("TenPhong")]
        [MaxLength(255)]
        public string RoomName { get; set; }

        [Column("LoaiPhong")]
        public int RoomType { get; set; }

        [Column("AnhPhong")]
        public string RoomImageURL { get; set; }

        [Column("status")]
        public bool Status { get; set; }
    }
}
