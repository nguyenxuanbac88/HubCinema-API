using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API_Project.Models.Entities
{
    [Table("Combo_Rap")]
    public class Combo_Cinema
    {
        [ForeignKey(nameof(Food))]
        public int MaDoAn { get; set; }

        [ForeignKey(nameof(Cinema))]
        public int MaRap { get; set; }

        public virtual Food Food { get; set; }
        public virtual Cinema Cinema { get; set; }
    }
}
