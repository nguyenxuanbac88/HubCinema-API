using StackExchange.Redis;
namespace API_Project.Services
{
    public class RedisService
    {
        private readonly IDatabase _redis;
        private const int ExpirationMinutes = 5;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        private string GetSetKey(int suatChieuId) => $"suatchieu:{suatChieuId}:held_seats";

        public async Task<bool> HoldSeatAsync(int suatChieuId, string maGhe)
        {
            string key = GetSetKey(suatChieuId);
            bool added = await _redis.SetAddAsync(key, maGhe);
            await _redis.KeyExpireAsync(key, TimeSpan.FromMinutes(ExpirationMinutes));
            return added;
        }

        public async Task<bool> IsSeatLockedAsync(int suatChieuId, string maGhe)
        {
            string key = GetSetKey(suatChieuId);
            return await _redis.SetContainsAsync(key, maGhe);
        }

        public async Task<bool> ReleaseSeatAsync(int suatChieuId, string maGhe)
        {
            string key = GetSetKey(suatChieuId);
            return await _redis.SetRemoveAsync(key, maGhe);
        }

        public async Task<List<string>> GetHeldSeatsAsync(int suatChieuId)
        {
            string key = GetSetKey(suatChieuId);
            var members = await _redis.SetMembersAsync(key);
            return members.Select(m => m.ToString()).ToList();
        }
    }

}
