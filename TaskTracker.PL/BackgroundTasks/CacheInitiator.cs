using StackExchange.Redis;
using TaskTracker.BLL.Cache;
using TaskTracker.BLL.Interfaces;
using TaskTrackerWeb.Entity;

namespace TaskTracker.PL.BackgroundTasks
{
    public class CacheInitiator
    {
        private ConnectionMultiplexer _connectionMultiplexer = RedisConnection.Connection;
        private ITaskTrackerBll _taskTrackerBLL;

        public CacheInitiator(ITaskTrackerBll taskTrackerBLL)
        {
            _taskTrackerBLL = taskTrackerBLL;
        }

        public async Task StartUserTasks()
        {
            var tasks = await _taskTrackerBLL.GetAllTasks();
            foreach (UserTask task in tasks)
            {
                _connectionMultiplexer.SetTask(task);
            }
        }

        public async Task StartUsers()
        {
            var users = await _taskTrackerBLL.GetAllUser();
            foreach (User user in users)
            {
                _connectionMultiplexer.SetUser(user);
            }
        }
    }
}
