using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskTracker.BLL.Interfaces;
using TaskTracker.PL.Models;
using TaskTrackerWeb.Entity;

namespace TaskTracker.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ITaskTrackerBll _taskTrackerLogic;

        public HomeController(ILogger<HomeController> logger, ITaskTrackerBll taskTrackerLogic)
        {
            _logger = logger;
            _taskTrackerLogic = taskTrackerLogic;
        }

        public async Task<IActionResult> GetUsersTask()
        {
            string login = User.Identity.Name;
            var user = await _taskTrackerLogic.GetAccount(login);
            var usersTask = await _taskTrackerLogic.GetUsersTasks(user.Id);
            if (usersTask.Count > 0)
            {
                var userTaskSelected = usersTask.Select(UserTaskModel.TaskFromEntity).ToList();
                return View(userTaskSelected);
            }
            ViewBag.Message = "You don't have any tasks yet";
            return View();
        }

        public async Task<IActionResult> GetUserAccount()
        {
            string login = User.Identity.Name;
            var account = await _taskTrackerLogic.GetAccount(login);
            var user = await _taskTrackerLogic.GetUser(account.Id);
            
            return View(SetAccountUser(account, user));
        }

        private AccountUserModel SetAccountUser(Account account, User user)
        {
            var accountUser = new AccountUserModel();
            accountUser.Id = user.Id;
            accountUser.Name = user.Name;
            accountUser.PhoneNumber = user.PhoneNumber;
            accountUser.Login = account.Login;
            accountUser.Password = account.Password;

            return accountUser;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}