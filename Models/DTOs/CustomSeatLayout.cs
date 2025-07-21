namespace API_Project.Models.DTOs
{
    public class CustomSeatLayout
    {
        public int IdRoom { get; set; }
        public string FileName { get; set; } = string.Empty;
        public List<List<string?>> Layout { get; set; } = new();
    }
}
