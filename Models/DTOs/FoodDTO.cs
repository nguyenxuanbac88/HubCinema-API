namespace API_Project.Models.DTOs
{
    public class FoodDTO
    {
        public int IDFood { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        public int IDCinema { get; set; }
    }
}
