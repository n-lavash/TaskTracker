using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.BLL.Interfaces;
using TaskTracker.PL.Models;

namespace TaskTracker.PL.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private ITaskTrackerBll _taskTrackerLogic;

        public TaskController(ILogger<TaskController> logger, ITaskTrackerBll taskTrackerLogic)
        {
            _logger = logger;
            _taskTrackerLogic = taskTrackerLogic;
        }

        public async Task<IActionResult> AddTask(UserTaskModel task)
        {
            if (ModelState.IsValid)
            {
                string login = User.Identity.Name;
                var user = await _taskTrackerLogic.GetAccount(login);
                var title = task.Title;
                var descriptionInfo = task.Description;
                var creationDate = DateTime.Now;
                var deadline = task.DeadLine;

                await _taskTrackerLogic.AddTask(user.Id, title, descriptionInfo, creationDate, deadline);
                return RedirectToAction("GetUsersTask", "Home");
            }

            return View(task);
        }

        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskTrackerLogic.DeleteTask(id);
            return RedirectToAction("GetUsersTask", "Home");
        }

        public async Task<IActionResult> EditTask(int id, UserTaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                var title = taskModel.Title;
                var description = taskModel.Description;
                var creationDate = DateTime.Now;
                var deadline = taskModel.DeadLine;

                await _taskTrackerLogic.EditTask(id, title, description, creationDate, deadline);
                return RedirectToAction("GetTask", "Home");
            }
            return View(taskModel);
        }

        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskTrackerLogic.GetTask(id);
            var taskModel = UserTaskModel.TaskFromEntity(task);
            return View(taskModel);
        }
    }
}
