using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_Project.Models.Entities
{
    [Table("Foods")]
    public class Food
    {
        [Key]
        [Column("IDFood")]
        public int IDFood { get; set; }

        [Column("FoodName")]
        [MaxLength(255)]
        public int FoodName { get; set; }

        [Column("Description")]
        [MaxLength(255)]
        public int Description { get; set; }

        [Column("Price")]
        public Decimal Price { get; set; }

        [Column("ImageURL")]
        public string ImageURL { get; set; }
    }
}
