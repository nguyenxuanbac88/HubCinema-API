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
                Actors = m.Actors
            })
            .ToListAsync();

        return movies;
    }
    public async Task<bool> CreateCinema(CinemaDTO cinemaDTO)
    {
        try
        {
            var cinema = new Cinema
            {
                CinemaName = cinemaDTO.CinemaName,
                Address = cinemaDTO.Address,
                City = cinemaDTO.City
            };
            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> CreateMovie(MovieDTO movieDTO)
    {
        try
        {
            var movie = new Movie
            {
                MovieName = movieDTO.MovieName,
                Genre = movieDTO.Genre,
                Duration = movieDTO.Duration,
                Description = movieDTO.Description,
                Director = movieDTO.Director,
                ReleaseDate = movieDTO.ReleaseDate,
                CoverURL = movieDTO.CoverURL,
                TrailerURL = movieDTO.TrailerURL,
                AgeRestriction = movieDTO.AgeRestriction,
                Producer = movieDTO.Producer,
                Actors = movieDTO.Actors
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
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
                IDCinema = m.CinemaID,
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
                RoomType = r.RoomType,
                RoomImageURL = r.RoomImageURL,
                Status = r.Status
            })
            .ToListAsync();
        return rooms;
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
    public async Task<bool> UpdateCinema(int id, CinemaDTO cinemaDTO)
    {
        var cinema = await _context.Cinemas.FindAsync(id);
        if(cinema == null)
        {
            return false;
        }
        cinema.CinemaName = cinemaDTO.CinemaName;
        cinema.Address = cinemaDTO.Address;
        cinema.City = cinemaDTO.City;
        _context.Cinemas.Update(cinema);
        await _context.SaveChangesAsync();
        return true;
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
                Actors = m.Actors
            })
            .FirstOrDefaultAsync();
        return movie;
    }
    public async Task<bool> UpdateMovieAsync(int id, MovieDTO movieDTO)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return false;
        }
        movie.MovieName = movieDTO.MovieName;
        movie.Genre = movieDTO.Genre;
        movie.Duration = movieDTO.Duration;
        movie.Description = movieDTO.Description;
        movie.Director = movieDTO.Director;
        movie.ReleaseDate = movieDTO.ReleaseDate;
        movie.CoverURL = movieDTO.CoverURL;
        movie.TrailerURL = movieDTO.TrailerURL;
        movie.AgeRestriction = movieDTO.AgeRestriction;
        movie.Producer = movieDTO.Producer;
        movie.Actors = movieDTO.Actors;
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
        return true;
    }
}
