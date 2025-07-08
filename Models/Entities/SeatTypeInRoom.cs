using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("LoaiGheTrongPhong")]
    public class SeatTypeInRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("MaRap")]
        public int CinemaId { get; set; }

        [Column("MaPhong")]
        public int RoomId { get; set; }

        [Column("MaGhe")]
        public string RowCode { get; set; }  

        [Column("LoaiGhe")]
        public string SeatType { get; set; } 

        [Column("Gia")]
        public long Price { get; set; }
    }
}
