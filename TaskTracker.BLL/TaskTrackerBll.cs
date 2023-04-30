using TaskTracker.BLL.Interfaces;
using TaskTracker.DAL.Interfaces;
using TaskTrackerWeb.Entity;

namespace TaskTracker.BLL
{
    public class TaskTrackerBll : ITaskTrackerBll
    {
        private ITaskTrackerDao _taskTrackerDAO;

        public TaskTrackerBll(ITaskTrackerDao taskTrackerDAO)
        {
            _taskTrackerDAO = taskTrackerDAO;
        }

        public async Task<bool> AddTask(int idUser, string title, string descriptionInfo, DateTime creationDate, DateTime deadline) =>
           await _taskTrackerDAO.AddTask(idUser, title, descriptionInfo, creationDate, deadline);

        public async Task<bool> AddUser(string name, string login, string password, string phoneNumber) =>
           await _taskTrackerDAO.AddUser(name, login, password, phoneNumber);

        public async Task<bool> CheckAccount(string login, string password) =>
            await _taskTrackerDAO.CheckAccount(login, password);

        public async Task<bool> DeleteTask(int id) =>
            await _taskTrackerDAO.DeleteTask(id);

        public async Task<bool> EditAccount(Account account) =>
            await _taskTrackerDAO.EditAccount(account);

        public async Task<bool> EditTask(int taskId, string title, string description, DateTime createdDate, DateTime deadline) =>
            await _taskTrackerDAO.EditTask(taskId, title, description, createdDate, deadline);

        public async Task<bool> EditUser(User user) =>
            await _taskTrackerDAO.EditUser(user);

        public async Task<Account> GetAccount(int id) =>
            await _taskTrackerDAO.GetAccount(id);

        public async Task<Account> GetAccount(string login) =>
            await _taskTrackerDAO.GetAccount(login);

        public async Task<UserTask> GetTask(int id) =>
            await _taskTrackerDAO.GetTask(id);

        public async Task<User> GetUser(int id) =>
            await _taskTrackerDAO.GetUser(id);

        public async Task<List<UserTask>> GetUsersTasks(int idUser) =>
            await _taskTrackerDAO.GetUsersTasks(idUser);
    }
}
