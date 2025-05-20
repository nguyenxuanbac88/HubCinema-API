using Microsoft.EntityFrameworkCore;

namespace API_Project.Controllers
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
            : base(options) { }
    }
}