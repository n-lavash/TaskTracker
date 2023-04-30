using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DAL.Interfaces;
using TaskTrackerWeb.Entity;

namespace TaskTracker.DAL
{
    public class TaskTrackerDao : ITaskTrackerDao
    {
        private string _connectionString;
        private SqlConnection _connection;

        public TaskTrackerDao(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<bool> AddTask(int idUser, string title, string descriptionInfo, DateTime creationDate, DateTime deadline)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                // TODO: не работает хранимая процедура
                var strProc = "TaskManager_AddTask";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    command.Parameters.AddWithValue("@ID_User", idUser);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@DescriptionInfo", descriptionInfo);
                    command.Parameters.AddWithValue("@CreatedDate", creationDate);
                    command.Parameters.AddWithValue("@Deadline", deadline);

                    _connection.Open();

                    var result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }



                throw new InvalidOperationException(
                    string.Format("Cannot add task with parameters: {0}, {1}, {2}, {3}",
                    title, descriptionInfo, creationDate, deadline));
            }
        }

        public async Task<bool> AddUser(string name, string login, string password, string phoneNumber)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProc = "TaskManager_AddUser";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@NameUser", name);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@PhoneNumber", Convert.ToDecimal(phoneNumber));

                _connection.Open();

                try
                {
                    var result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }

                throw new InvalidOperationException(
                    string.Format("Cannot add user with parameters: {0}, {1}, {2}",
                    name, login, phoneNumber));
            }
        }

        public async Task<bool> CheckAccount(string login, string password)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProc = "TaskManager_CheckAccount";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Login", login);

                _connection.Open();

                var result = command.ExecuteScalar();

                return result.Equals(1);

                throw new InvalidOperationException(
                    string.Format("Cannot check user with parameters: {0}, {1}",
                    login, password));
            }
        }

        public async Task<bool> DeleteTask(int id)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProc = "TaskManager_DeleteTask";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID", id);

                _connection.Open();

                try
                {
                    // TODO: удаляет нужную строку,но ExecuteNonQuerry() всегда возвращает -1
                    var result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                throw new InvalidOperationException(
                    string.Format("Cannot delete task by id: {0}",
                    id));
            }
        }

        public async Task<bool> EditAccount(Account account)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProcUser = "TaskManager_EditAccount";

                var command = new SqlCommand(strProcUser, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID", account.Id);
                command.Parameters.AddWithValue("@Login", account.Login);
                command.Parameters.AddWithValue("@Password", account.Password);

                _connection.Open();
                try
                {
                    // TODO: ExecuteNonQuerry() всегда возвращает -1
                    var result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> EditTask(int taskId, string title, string description, DateTime createdDate, DateTime deadline)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProcUser = "TaskManager_EditTask";

                var command = new SqlCommand(strProcUser, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                
                try
                {
                    command.Parameters.AddWithValue("@ID_Task", taskId);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@DescriptionInfo", description);
                    command.Parameters.AddWithValue("@CreatedDate", createdDate);
                    command.Parameters.AddWithValue("@Deadline", deadline);

                    _connection.Open();

                    var result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> EditUser(User user)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProcUser = "TaskManager_EditUser";

                var command = new SqlCommand(strProcUser, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                
                try
                {
                    command.Parameters.AddWithValue("@ID_User", user.Id);
                    command.Parameters.AddWithValue("@NameUser", user.Name);
                    command.Parameters.AddWithValue("@PhoneNumber", Convert.ToDecimal(user.PhoneNumber));

                    _connection.Open();

                    var result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<Account> GetAccount(int id)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProc = "TaskManager_GetAccountById";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_User", id);

                _connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Account(
                        id: Convert.ToInt32(reader["ID_Account"]),
                        login: reader["Login"] as string,
                        password: reader["Password"] as string);
                }

                throw new InvalidOperationException("Cannot find Account whith ID = " + id);
            }
        }
        public async Task<Account> GetAccount(string login)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProc = "TaskManager_GetAccountByLogin";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Login", login);

                _connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Account(
                        id: Convert.ToInt32(reader["ID_Account"]),
                        login: reader["Login"] as string,
                        password: reader["Password"] as string);
                }

                throw new InvalidOperationException("Cannot find Account whith login = " + login);
            }
        }

        public async Task<UserTask> GetTask(int id)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProc = "TaskManager_GetTaskById";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_Task", id);

                _connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new UserTask(
                        id: Convert.ToInt32(reader["ID_Task"]),
                        title: reader["Title"] as string,
                        description: reader["DescriptionInfo"] as string,
                        createdDate: Convert.ToDateTime(reader["CreatedDate"]),
                        deadline: Convert.ToDateTime(reader["Deadline"]));
                }

                throw new InvalidOperationException("Cannot find Task whith ID = " + id);
            }
        }

        public async Task<User> GetUser(int id)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProc = "TaskManager_GetUserById";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_User", id);

                _connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new User(
                        id: Convert.ToInt32(reader["ID_User"]),
                        name: reader["NameUser"] as string,
                        phoneNumber: (reader["PhoneNumber"]).ToString());
                }

                throw new InvalidOperationException("Cannot find User whith ID = " + id);
            }
        }

        public async Task<List<UserTask>> GetUsersTasks(int idUser)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                var strProc = "TaskManager_GetUserTasks";

                var command = new SqlCommand(strProc, _connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_User", idUser);

                _connection.Open();

                var reader = command.ExecuteReader();

                var tasks = new List<UserTask>();

                while (reader.Read())
                {
                    tasks.Add(new UserTask(
                        id: Convert.ToInt32(reader["ID_Task"]),
                        title: reader["Title"] as string,
                        description: reader["DescriptionInfo"] as string,
                        createdDate: Convert.ToDateTime(reader["CreatedDate"]),
                        deadline: Convert.ToDateTime(reader["Deadline"]))
                        );

                }

                return tasks;
            }
        }
    }
}
