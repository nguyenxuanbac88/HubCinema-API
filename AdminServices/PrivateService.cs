using API_Project.Data;
using API_Project.Models.DTOs;
using API_Project.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_Project.AdminServices
{
    public class PrivateService
    {
        private readonly ApplicationDbContext _context;
        public PrivateService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Cinema
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
        public async Task<bool> UpdateCinema(int id, CinemaDTO cinemaDTO)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
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
        //Movie
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

        //Room
        public async Task<List<RoomDTO>> GetRoomsByCinemaAsync(string nameCinema)
        {
            var room = await _context.Rooms
                .Include(r => r.Cinema)
                .Where(r => r.Cinema.CinemaName == nameCinema)
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
            return room;
        }
        public async Task<bool> CreateRoom(RoomDTO roomDTO)
        {
            try
            {
                var room = new Room
                {
                    IDRoom = roomDTO.IDRoom,
                    CinemaID = roomDTO.CinemaID,
                    RoomName = roomDTO.RoomName,
                    RoomType = roomDTO.RoomType,
                    RoomImageURL = roomDTO.RoomImageURL,
                    Status = roomDTO.Status
                };
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Food
        public async Task<bool> CreateFood(FoodDTO foodDTO)
        {
            try
            {
                var food = new Food
                {
                    FoodName = foodDTO.FoodName,
                    Price = foodDTO.Price,
                    Description = foodDTO.Description,
                    ImageURL = foodDTO.ImageURL,
                    CinemaID = foodDTO.IDCinema
                };
                _context.Foods.Add(food);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
