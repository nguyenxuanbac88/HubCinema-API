using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_Project.Models.Entities
{
    [Table("DoAn")]
    public class Food
    {
        [Key]
        [Column("MaDoAn")]
        public int? IDFood { get; set; }

        [Column("TenDoAn")]
        public string FoodName { get; set; }

        [Column("MoTa")]
        public string Description { get; set; }

        [Column("GiaTien")]
        public Decimal Price { get; set; }

        [Column("URL")]
        public string ImageURL { get; set; }
        public virtual ICollection<Combo_Cinema> ComboCinemas { get; set; }

    }
}
