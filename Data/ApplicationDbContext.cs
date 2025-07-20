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
        public DbSet<Combo_Cinema> Combo_Cinema { get; set; }
        public DbSet<ShowtimeType> ShowtimeTypes { get; set; }
        public DbSet<SeatTypeInRoom> SeatTypesInRooms { get; set; }

        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<BookedSeat> BookedSeats { get; set; }
        public DbSet<InvoiceFood> InvoiceFoods { get; set; }

        public object ShowtimeType { get; internal set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Banner> Banners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Combo_Cinema>()
                .HasKey(cc => new { cc.MaDoAn, cc.MaRap });

            modelBuilder.Entity<Combo_Cinema>()
                .HasOne(cc => cc.Food)
                .WithMany(f => f.ComboCinemas)
                .HasForeignKey(cc => cc.MaDoAn);

            modelBuilder.Entity<Combo_Cinema>()
                .HasOne(cc => cc.Cinema)
                .WithMany(c => c.ComboCinemas)
                .HasForeignKey(cc => cc.MaRap);
        }
        public DbSet<News> News { get; set; }

    }
}
