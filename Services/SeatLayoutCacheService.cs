using System.Text.Json;
using StackExchange.Redis;
using API_Project.Models.DTOs;
using API_Project.Services.Interfaces;

namespace API_Project.Services
{
    public class SeatLayoutCacheService : ISeatLayoutCacheService
    {
        private readonly IDatabase _redis;

        public SeatLayoutCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<Dictionary<string, SeatLayoutItemDto>> GetSeatLayoutAsync(int showtimeId)
        {
            var redisKey = $"seat-layout:{showtimeId}";
            var json = await _redis.StringGetAsync(redisKey);

            if (json.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<Dictionary<string, SeatLayoutItemDto>>(json);
        }
    }
}
