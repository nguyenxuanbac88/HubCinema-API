using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Project.Models.Entities
{
    [Table("TatCaGhePhongPhim")]
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MaGhePhongPhim")]
        public int Id { get; set; }

        [Column("MaPhong")]
        public int IdRoom { get; set; }

        [Column("LoaiGhe")]
        public int IdTypeSeat { get; set; }

        [Column("GiaTienPhanTram")]
        public int PriceInPercentage { get; set; }

        [Column("GiaTien")]
        public int Price { get; set; }

        [Column("sothutu")]
        public int Number { get; set; }
    }
}
