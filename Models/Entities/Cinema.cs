using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("Cinemas")]
    public class Cinema
    {
        [Key]
        [Column("IDCinema")]
        public int IDCinema { get; set; }

        [Column("CinemaName")]
        [MaxLength(255)]
        public string CinemaName { get; set; }

        [Column("Address")]
        public string Address { get; set; }


        [Column("City")]
        [MaxLength(255)]
        public string City { get; set; }

    }
}
