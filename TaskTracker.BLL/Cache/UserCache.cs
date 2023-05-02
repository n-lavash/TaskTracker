using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskTracker.BLL.Cache
{
    public static class UserCache
    {
        public static void SetUser(this ConnectionMultiplexer connectionMultiplexer, TaskTrackerWeb.Entity.User user)
        {
            var userJson = JsonSerializer.Serialize(user);
            connectionMultiplexer.GetDatabase().StringSet(user.Id.ToString(), userJson);
        }

        public static List<TaskTrackerWeb.Entity.User> GetUsers(this ConnectionMultiplexer connectionMultiplexer)
        {
            RedisKey[] keys = connectionMultiplexer.GetServer("redis:6379").Keys().ToArray();
            var values = connectionMultiplexer.GetDatabase().StringGet(keys);
            return values.Select(key => JsonSerializer.Deserialize<TaskTrackerWeb.Entity.User>(key!)!).ToList();
        }
    }
}
