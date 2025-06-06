// MovieService.cs
using API_Project.Data;
using API_Project.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PublicService
{
    private readonly ApplicationDbContext _context;

    public PublicService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovieDTO>> GetAllMoviesAsync()
    {
        var movies = await _context.Movies
            .Select(m => new MovieDTO
            {
                IDMovie = m.IDMovie,
                MovieName = m.MovieName,
                Genre = m.Genre,
                Duration = m.Duration,
                Description = m.Description,
                Director = m.Director,
                ReleaseDate = m.ReleaseDate,
                CoverURL = m.CoverURL,
                TrailerURL = m.TrailerURL
            })
            .ToListAsync();

        return movies;
    }
    public async Task<List<FoodDTO>> GetAllFoodsAsync()
    {
        var foods = await _context.Foods
            .Select(m => new FoodDTO
            {
                IDFood = m.IDFood,
                FoodName = m.FoodName,
                Description = m.Description,
                Price = m.Price,
                ImageURL = m.ImageURL,
            })
            .ToListAsync();

        return foods;
    }
    public async Task<List<CinemaDTO>> GetAllCinemaAsync()
    {
        var cinemas = await _context.Cinemas
            .Select(c => new CinemaDTO
            {
                IDCinema = c.IDCinema,
                CinemaName = c.CinemaName,
                Address = c.Address,
                City = c.City
            })
            .ToListAsync();
        return cinemas;
    }
    public async Task<List<RoomDTO>> GetAllRoomAsync()
    {
        var rooms = await _context.Rooms
            .Select(r => new RoomDTO
            {
                IDRoom = r.IDRoom,
                CinemaID = r.CinemaID,
                RoomName = r.RoomName,
                TotalSeats = r.TotalSeats,
                TicketPriceID = r.TicketPriceID,
                ImageURL = r.ImageURL
            })
            .ToListAsync();
        return rooms;
    }
}
