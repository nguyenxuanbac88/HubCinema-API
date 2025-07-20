using API_Project.Data;
using API_Project.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Services
{
    public class BannerService
    {
        private readonly ApplicationDbContext _context;

        public BannerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Banner>> GetActiveBannersAsync()
        {
            return await _context.Banners
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Banner?> GetByIdAsync(int id)
        {
            return await _context.Banners.FindAsync(id);
        }

        public async Task<bool> AddBannerAsync(Banner banner)
        {
            banner.CreatedAt = DateTime.Now;
            _context.Banners.Add(banner);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateBannerAsync(Banner banner)
        {
            var existing = await _context.Banners.FindAsync(banner.BannerId);
            if (existing == null) return false;

            existing.Title = banner.Title;
            existing.ImageUrl = banner.ImageUrl;
            existing.LinkUrl = banner.LinkUrl;
            existing.DisplayOrder = banner.DisplayOrder;
            existing.IsActive = banner.IsActive;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) return false;

            _context.Banners.Remove(banner);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
