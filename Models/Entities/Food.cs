using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_Project.Models.Entities
{
    [Table("Food")]
    public class Food
    {
        [Key]
        [Column("IDFood")]
        public int IDFood { get; set; }

        [Column("FoodName")]
        public string FoodName { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Price")]
        public Decimal Price { get; set; }

        [Column("ImageURL")]
        public string ImageURL { get; set; }

        [Column("CinemaID")]
        public int CinemaID { get; set; }
    }
}
