using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Subtitle { get; set; }
        public int Category { get; set; }
        public string? Slug { get; set; }
        public string? Thumbnail { get; set; }
        public string? Content { get; set; }
        public DateTime? CeateAt { get; set; }
        public string? Status { get; set; }
    }
    [Table("CategoryNews")]
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Extra { get; set; }
        public string? Status { get; set; } = "true";
        public string? Slug { get; set; }
    }

}
