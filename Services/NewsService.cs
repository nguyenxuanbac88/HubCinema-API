using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Project.Data;
using API_Project.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Services
{
    public class NewsService
    {
        private readonly ApplicationDbContext _context;

        public NewsService(ApplicationDbContext context)
        {
            _context = context;
        }

        // --------- NEWS CRUD ---------
        public async Task<List<News>> GetAllAsync()
        {
            return await _context.News.ToListAsync();
        }

        public async Task<News?> GetByIdAsync(int id)
        {
            return await _context.News.FindAsync(id);
        }

        public async Task<News> CreateAsync(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public async Task<bool> UpdateAsync(News news)
        {
            if (!_context.News.Any(e => e.Id == news.Id)) return false;
            _context.News.Update(news);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return false;
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return true;
        }

        // --------- CATEGORY ---------
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.Status == "true")
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(long id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
