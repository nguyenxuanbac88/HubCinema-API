using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("LoaiSuatChieu")]
    public class ShowtimeType
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("type")]
        public byte TypeName { get; set; }

        [Column("GiaTien")]
        public long Price { get; set; }

        public ICollection<Showtime> Showtimes { get; set; }
    }
}
