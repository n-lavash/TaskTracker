using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackerWeb.Entity;

namespace TaskTracker.DAL.Interfaces
{
    public interface ITaskTrackerDao
    {
        Task<UserTask> AddTask(int idUser, string title, string descriptionInfo, DateTime creationDate, DateTime deadline);

        Task<User> AddUser(string name, string login, string password, string phoneNumber);

        Task<bool> CheckAccount(string login, string password);

        Task<bool> DeleteTask(int id);

        Task<bool> EditAccount(Account account);

        Task<UserTask> EditTask(int taskId, string title, string description, DateTime createdDate, DateTime deadline);

        Task<User> EditUser(User user);

        Task<Account> GetAccount(string login);

        Task<Account> GetAccount(int id);

        Task<UserTask> GetTask(int id);

        Task<User> GetUser(int id);

        Task<List<UserTask>> GetUsersTasks(int idUser);
        Task<List<UserTask>> GetAllTasks();
        Task<List<User>> GetAllUser();
    }
}
