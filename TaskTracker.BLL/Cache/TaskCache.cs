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
    public static class TaskCache
    {
        public static void SetTask(this ConnectionMultiplexer connectionMultiplexer, UserTask task)
        {
            var taskJson = JsonSerializer.Serialize(task);
            connectionMultiplexer.GetDatabase().StringSet(task.Id.ToString() + "task", taskJson);
        }

        public static void DeleteTask(this ConnectionMultiplexer connectionMultiplexer, int id)
        {
            connectionMultiplexer.GetDatabase().KeyDelete(id.ToString()+"task");
        }

        public static List<UserTask> GetTasks(this ConnectionMultiplexer connectionMultiplexer)
        {
            RedisKey[] keys = connectionMultiplexer.GetServer("redis:6379").Keys().Where(key => key.ToString().EndsWith("task")).ToArray();
            var values = connectionMultiplexer.GetDatabase().StringGet(keys);
            return values.Select(key => JsonSerializer.Deserialize<UserTask>(key!)!).ToList();
        }
    }
}
