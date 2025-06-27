using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("SuatChieu")]
    public class Showtime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MaSuatChieu")]
        public int MaSuatChieu { get; set; }

        [Column("PhongChieu")]
        public int PhongChieu { get; set; }

        [Column("MaPhim")]
        public int MaPhim { get; set; }

        [Column("NgayChieu")]
        public DateTime NgayChieu { get; set; }

        [Column("GioChieu")]
        public TimeSpan GioChieu { get; set; }

        [Column("ChiPhi")]
        public int ChiPhi { get; set; }

        [Column("TypeSuatChieu")]
        public int TypeSuatChieu { get; set; }

        [Column("MaRap")]
        public int MaRap { get; set; }

        [ForeignKey("MaPhim")]
        public Movie Movie { get; set; }

        [ForeignKey("MaRap")]
        public Cinema Cinema { get; set; }
    }
}
