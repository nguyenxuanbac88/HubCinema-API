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
        public async Task<bool> DeleteCinemaAsync(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return false;
            }
            try
            {
                _context.Cinemas.Remove(cinema);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
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
                    EndDate = movieDTO.EndDate,
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
        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return false;
            }
            try
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Room
        public async Task<List<RoomDTO>> GetRoomsByCinemaAsync(int idCinema)
        {
            var room = await _context.Rooms
                .Include(r => r.Cinema)
                .Where(r => r.Cinema.IDCinema == idCinema)
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
            catch (Exception ex)
            {
                Console.WriteLine("Error creating room: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdateRoomAsync(int id, RoomDTO roomDTO)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return false;
            }
            room.CinemaID = roomDTO.CinemaID;
            room.RoomName = roomDTO.RoomName;
            room.RoomType = roomDTO.RoomType;
            room.RoomImageURL = roomDTO.RoomImageURL;
            room.Status = roomDTO.Status;
            room.id_layout = roomDTO.id_layout;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return false;
            }
            try
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Food
        public async Task<FoodDTO> CreateFood(FoodDTO foodDTO)
        {
            try
            {
                var food = new Food
                {
                    FoodName = foodDTO.FoodName,
                    Price = foodDTO.Price,
                    Description = foodDTO.Description,
                    ImageURL = foodDTO.ImageURL,
                };

                _context.Foods.Add(food);
                await _context.SaveChangesAsync();

                return new FoodDTO
                {
                    IDFood = food.IDFood,
                    FoodName = food.FoodName,
                    Price = food.Price,
                    Description = food.Description,
                    ImageURL = food.ImageURL,
                };
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> UpdateFoodAsync(int id, FoodDTO foodDTO)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return false;
            }
            food.FoodName = foodDTO.FoodName;
            food.Price = foodDTO.Price;
            food.Description = foodDTO.Description;
            food.ImageURL = foodDTO.ImageURL;
            _context.Foods.Update(food);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteFoodAsync(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return false;
            }
            try
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> CreateComboForCinemasAsync(CreateComboCinema dto)
        {
            try
            {
                foreach (var maRap in dto.IdCinemapList)
                {
                    var exists = await _context.Combo_Cinema
                        .AnyAsync(c => c.MaDoAn == dto.IdFood && c.MaRap == maRap);

                    if (!exists)
                    {
                        var combo = new Combo_Cinema
                        {
                            MaDoAn = dto.IdFood,
                            MaRap = maRap
                        };

                        _context.Combo_Cinema.Add(combo);
                    }
                }

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
