// MovieService.cs
using API_Project.Data;
using API_Project.Models.DTOs;
using API_Project.Models.Entities;
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

    //Cinema
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
    public async Task<CinemaDTO?> GetCinemaByIdAsync(int id)
    {
        var cinema = await _context.Cinemas
            .Where(c => c.IDCinema == id)
            .Select(c => new CinemaDTO
            {
                IDCinema = c.IDCinema,
                CinemaName = c.CinemaName,
                Address = c.Address,
                City = c.City
            })
            .FirstOrDefaultAsync();
        return cinema;
    }

    //MOVIE
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
                TrailerURL = m.TrailerURL,
                AgeRestriction = m.AgeRestriction,
                Producer = m.Producer,
                Actors = m.Actors,
                EndDate = m.EndDate

            })
            .ToListAsync();

        return movies;
    }
    public async Task<MovieDTO?> GetMovieByIdAsync(int id)
    {
        var movie = await _context.Movies
            .Where(m => m.IDMovie == id)
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
                TrailerURL = m.TrailerURL,
                AgeRestriction = m.AgeRestriction,
                Producer = m.Producer,
                Actors = m.Actors,
                EndDate = m.EndDate
            })
            .FirstOrDefaultAsync();
        return movie;
    }

    //FOOD
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
    public async Task<FoodDTO?> GetFoodByIdAsync(int id)
    {
        var food = await _context.Foods
            .Where(f => f.IDFood == id)
            .Select(f => new FoodDTO
            {
                IDFood = f.IDFood,
                FoodName = f.FoodName,
                Description = f.Description,
                Price = f.Price,
                ImageURL = f.ImageURL,
            })
            .FirstOrDefaultAsync();
        return food;
    }

    //Room
    public async Task<List<RoomDTO>> GetAllRoomAsync()
    {
        var rooms = await _context.Rooms
            .Select(r => new RoomDTO
            {
                IDRoom = r.IDRoom,
                CinemaID = r.CinemaID,
                RoomName = r.RoomName,
                RoomType = r.RoomType,
                RoomImageURL = r.RoomImageURL,
                Status = r.Status
            })
            .ToListAsync();
        return rooms;
    }
    public async Task<RoomDTO?> GetRoomByIdAsync(int id)
    {
        var room = await _context.Rooms
             .Where(m => m.IDRoom == id)
             .Select(m => new RoomDTO
             {
                 IDRoom = m.IDRoom,
                 CinemaID= m.CinemaID,
                 RoomName = m.RoomName,
                 RoomType = m.RoomType,
                 RoomImageURL = m.RoomImageURL,
                 Status = m.Status
             }).FirstOrDefaultAsync();
        return room;
    }
    public List<Food> GetCombosByCinema(int cinemaId)
    {
        return _context.Combo_Cinema
            .Where(c => c.MaRap == cinemaId)
            .Include(c => c.Food)
            .Select(c => c.Food)
            .ToList();
    }
}
