using API_Project.Models;
using API_Project.Models.Entities;
using Microsoft.EntityFrameworkCore;
using static API_Project.Models.Entities.User;

namespace API_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public DbSet<ShowtimeType> ShowtimeTypes { get; set; }
        public DbSet<SeatTypeInRoom> SeatTypesInRooms { get; set; }

        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<BookedSeat> BookedSeats { get; set; }
        public DbSet<InvoiceFood> InvoiceFoods { get; set; }

        public object ShowtimeType { get; internal set; }
        

    }
}
