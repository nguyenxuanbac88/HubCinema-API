using Microsoft.EntityFrameworkCore;

namespace API_Project.Data 
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        // public DbSet<User> Users { get; set; }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
