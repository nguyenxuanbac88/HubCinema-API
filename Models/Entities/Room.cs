using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_Project.Models.Entities
{
    [Table("Cinema_rooms")]
    public class Room
    {
        [Key]
        [Column("IDRoom")]
        public int IDRoom { get; set; }

        [Column("CinemaID")]
        public int CinemaID { get; set; }

        [Column("RoomName")]
        [MaxLength(255)]
        public string RoomName { get; set; }

        [Column("TotalSeats")]
        public int TotalSeats { get; set; }

        [Column("TicketPriceID")]
        public int TicketPriceID { get; set; }

        [Column("ImageURL")]
        public string ImageURL { get; set; }
    }
}
