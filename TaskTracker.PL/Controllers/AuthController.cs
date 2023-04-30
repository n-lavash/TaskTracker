using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskTracker.BLL.Interfaces;
using TaskTracker.PL.Models;

namespace TaskTracker.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        private ITaskTrackerBll _taskTrackerBll;

        public AuthController(ILogger<AuthController> logger, ITaskTrackerBll taskTrackerBll)
        {
            _logger = logger;
            _taskTrackerBll = taskTrackerBll;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                if (await IsValidAccountData(accountModel.Login, accountModel.Password))
                {
                    await Authenticate(accountModel.Login);

                    return RedirectToAction("GetUsersTask", "Home");
                }
                ModelState.AddModelError("", "Неверные логин или пароль.");
            }

            return View(accountModel);
        }

        [HttpGet]
        public IActionResult Registered()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registered(AccountUserModel model)
        {
            if (ModelState.IsValid)
            {
                var name = model.Name;
                var phoneNumber = model.PhoneNumber;
                var login = model.Login;
                var password = model.Password;

                if (name != null && phoneNumber != null && login != null && password != null)
                {
                    await _taskTrackerBll.AddUser(name, login, password, phoneNumber);


                    await Authenticate(model.Login);
                    return RedirectToAction("GetUsersTask", "Home");
                }
                else
                    ModelState.AddModelError("", "Все поля должны быть заполнены.");
            }

            return View();

        }

        private async Task<bool> IsValidAccountData(string login, string password)
        {

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;
            if (!(await _taskTrackerBll.CheckAccount(login, password)))
                return false;

            return true;
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
