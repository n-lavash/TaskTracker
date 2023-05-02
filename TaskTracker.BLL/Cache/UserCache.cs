using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskTrackerWeb.Entity;

namespace TaskTracker.BLL.Cache
{
    public static class UserCache
    {
        public static void SetUser(this ConnectionMultiplexer connectionMultiplexer, User user)
        {
            var userJson = JsonSerializer.Serialize(user);
            connectionMultiplexer.GetDatabase().StringSet(user.Id.ToString() + "user", userJson);
        }

        public static List<User> GetUsers(this ConnectionMultiplexer connectionMultiplexer)
        {
            RedisKey[] keys = connectionMultiplexer.GetServer("redis:6379").Keys().Where(key => key.ToString().EndsWith("user")).ToArray();
            var values = connectionMultiplexer.GetDatabase().StringGet(keys);
            return values.Select(key => JsonSerializer.Deserialize<User>(key!)!).ToList();
        }
    }
}
