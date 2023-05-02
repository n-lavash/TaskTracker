using StackExchange.Redis;
using TaskTracker.BLL.Cache;
using TaskTracker.BLL.Interfaces;
using TaskTrackerWeb.Entity;

namespace TaskTracker.PL.BackgroundTasks
{
    public class CheckTasks
    {
        private ConnectionMultiplexer _connectionMultiplexer = RedisConnection.Connection;
        private ITaskTrackerBll _taskTrackerBll;

        public CheckTasks(ITaskTrackerBll taskTrackerBll)
        {
            _taskTrackerBll = taskTrackerBll;
        }

        public async Task StartCheck()
        {
            var usersFromRedis = _connectionMultiplexer.GetUsers();
            var usersFromDB = await _taskTrackerBll.GetAllUser();

            foreach (User user in usersFromDB)
            {
                var equalUser = usersFromRedis.FirstOrDefault(x =>
                    x.Id == user.Id &&
                    x.Name == user.Name &&
                    x.PhoneNumber == user.PhoneNumber);

                if (equalUser == null)
                {
                    Console.WriteLine($"User (ID = {user.Id}) из базы данных не существует в redis");
                    return;
                }
            }

            var tasksFromRedis = _connectionMultiplexer.GetTasks();
            var tasksFromDB = await _taskTrackerBll.GetAllTasks();

            foreach (UserTask task in tasksFromDB)
            {
                var equalTask = tasksFromRedis.FirstOrDefault(x =>
                    x.Id == task.Id &&
                    x.Title == task.Title &&
                    x.Description == task.Description &&
                    x.CreatedDate == task.CreatedDate &&
                    x.Deadline == task.Deadline);

                if (equalTask == null)
                {
                    Console.WriteLine($"Task (ID = {task.Id}) из базы данных не существует в redis");
                    return;
                }
            }

            Console.WriteLine($"Данные в базе данных и в redis совпадают");
        }
    }
}
