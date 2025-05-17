using StackExchange.Redis;

namespace Order.Services
{
    public class RedisHelper
    {
        private readonly IDatabase _db;

        public RedisHelper(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
            //清空redis
            _db.ExecuteAsync("FLUSHDB").Wait(); // 同步等待异步操作完成
        }
        public bool Exists(string key)
        {
            return _db.KeyExists(key);
        }
        // 尝试获取锁（幂等）
        public bool TryAcquireLock(string key, TimeSpan expiration)
        {
            return _db.StringSet(key, "1", expiration, When.NotExists);
        }
    }
}