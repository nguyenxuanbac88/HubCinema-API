using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("RapPhim")]
    public class Cinema
    {
        [Key]
        [Column("MaRap")]
        public int IDCinema { get; set; }

        [Column("TenRap")]
        [MaxLength(255)]
        public string CinemaName { get; set; }

        [Column("DiaChi")]
        public string Address { get; set; }


        [Column("ThanhPho")]
        [MaxLength(255)]
        public string City { get; set; }
        [Column("TrangThai")]
        public int IsActive { get; set; }


        public ICollection<Room> Rooms { get; set; }
        public ICollection<Showtime> Showtimes { get; set; }
        public virtual ICollection<Combo_Cinema> ComboCinemas { get; set; }


    }
}
