using StackExchange.Redis;
namespace API_Project.Services
{
    public class RedisService
    {
        private readonly IDatabase _redis;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<bool> HoldSeatAsync(int suatChieuId, string maGhe, string maNguoiDung)
        {
            string key = $"seatlock:{suatChieuId}:{maGhe}";
            return await _redis.StringSetAsync(key, maNguoiDung, TimeSpan.FromMinutes(5), When.NotExists);
        }

        public async Task<bool> IsSeatLockedAsync(int suatChieuId, string maGhe)
        {
            string key = $"seatlock:{suatChieuId}:{maGhe}";
            return await _redis.KeyExistsAsync(key);
        }

        public async Task<bool> ReleaseSeatAsync(int suatChieuId, string maGhe)
        {
            string key = $"seatlock:{suatChieuId}:{maGhe}";
            return await _redis.KeyDeleteAsync(key);
        }

        public async Task<List<string>> GetHeldSeatsAsync(int showtimeId)
        {
            var heldSeats = new List<string>();

            var endpoints = _redis.Multiplexer.GetEndPoints();
            var server = _redis.Multiplexer.GetServer(endpoints[0]);

            var keys = server.Keys(pattern: $"seatlock:{showtimeId}:*");

            foreach (var key in keys) // ❗ dùng foreach thường, không dùng await
            {
                var parts = key.ToString().Split(':');
                if (parts.Length == 3)
                {
                    heldSeats.Add(parts[2]);
                }
            }

            return heldSeats;
        }
    }
}
