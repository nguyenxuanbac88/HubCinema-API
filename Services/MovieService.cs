// MovieService.cs
using API_Project.Data;
using API_Project.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MovieService
{
    private readonly ApplicationDbContext _context;

    public MovieService(ApplicationDbContext context)
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
}
