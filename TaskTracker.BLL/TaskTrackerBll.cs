using StackExchange.Redis;
using System.Threading.Tasks;
using TaskTracker.BLL.Cache;
using TaskTracker.BLL.Interfaces;
using TaskTracker.DAL.Interfaces;
using TaskTrackerWeb.Entity;

namespace TaskTracker.BLL
{
    public class TaskTrackerBll : ITaskTrackerBll
    {
        private ITaskTrackerDao _taskTrackerDAO;
        private ConnectionMultiplexer _connectionMultiplexer = RedisConnection.Connection;
        private object _lock = new();

        public TaskTrackerBll(ITaskTrackerDao taskTrackerDAO)
        {
            _taskTrackerDAO = taskTrackerDAO;
        }

        public async Task<UserTask> AddTask(int idUser, string title, string descriptionInfo, DateTime creationDate, DateTime deadline)
        {
            var task = await _taskTrackerDAO.AddTask(idUser, title, descriptionInfo, creationDate, deadline);
            try
            {
                CacheModifiedTask(task);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Невозможно добавить задачу в кеш: {e}");
            }

            return task;
        }

        public async Task<User> AddUser(string name, string login, string password, string phoneNumber)
        {
            var user = await _taskTrackerDAO.AddUser(name, login, password, phoneNumber);
            try
            {
                CacheModifiedUser(user);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Невозможно добавить пользователя в кеш: {e}");
            }

            return user;
        }

        public async Task<bool> CheckAccount(string login, string password) =>
            await _taskTrackerDAO.CheckAccount(login, password);

        public async Task<bool> DeleteTask(int id)
        {
            var result = await _taskTrackerDAO.DeleteTask(id);

            try
            {
                _connectionMultiplexer.DeleteTask(id);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Невозможно добавить задачу в кеш: {e}");
            }

            return result;
        }


        public async Task<bool> EditAccount(Account account) =>
            await _taskTrackerDAO.EditAccount(account);

        public async Task<UserTask> EditTask(int taskId, string title, string description, DateTime createdDate, DateTime deadline)
        {
            var task = await _taskTrackerDAO.EditTask(taskId, title, description, createdDate, deadline);
            try
            {
                CacheModifiedTask(task);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Невозможно добавить задачу в кеш: {e}");
            }

            return task;
        }

        public async Task<User> EditUser(User user)
        {
            var editUser = await _taskTrackerDAO.EditUser(user);

            try
            {
                CacheModifiedUser(editUser);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Невозможно добавить пользователя в кеш: {e}");
            }

            return editUser;
        }


        public async Task<Account> GetAccount(int id) =>
            await _taskTrackerDAO.GetAccount(id);

        public async Task<Account> GetAccount(string login) =>
            await _taskTrackerDAO.GetAccount(login);

        public async Task<List<UserTask>> GetAllTasks() =>
            await _taskTrackerDAO.GetAllTasks();

        public async Task<List<User>> GetAllUser() =>
            await _taskTrackerDAO.GetAllUser();

        public async Task<UserTask> GetTask(int id) =>
            await _taskTrackerDAO.GetTask(id);

        public async Task<User> GetUser(int id) =>
            await _taskTrackerDAO.GetUser(id);

        public async Task<List<UserTask>> GetUsersTasks(int idUser) =>
            await _taskTrackerDAO.GetUsersTasks(idUser);

        private void CacheModifiedTask(UserTask task)
        {
            lock(_lock)
            {
                _connectionMultiplexer.SetTask(task);
            }
        }

        private void CacheModifiedUser(User user)
        {
            lock(_lock)
            {
                _connectionMultiplexer.SetUser(user);
            }
        }
    }
}
