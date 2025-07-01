using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Project.Models.DTOs
{
    public class Seat
    {
        public int Id { get; set; }
        public int IdRoom { get; set; }
        public int IdTypeSeat { get; set; }
        public int PriceInPercentage { get; set; }
        public int Price { get; set; }
        public int Number { get; set; }
    }
}
