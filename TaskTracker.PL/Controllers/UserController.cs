using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskTracker.BLL.Interfaces;
using TaskTracker.PL.Models;
using TaskTrackerWeb.Entity;

namespace TaskTracker.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private ITaskTrackerBll _taskTrackerLogic;

        public UserController(ILogger<UserController> logger, ITaskTrackerBll taskTrackerLogic)
        {
            _logger = logger;
            _taskTrackerLogic = taskTrackerLogic;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> EditAccount(AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                var oldLogin = User.Identity.Name;
                var oldAccount = await _taskTrackerLogic.GetAccount(oldLogin);
                var login = accountModel.Login;
                var password = accountModel.Password;

                var account = new Account(oldAccount.Id, login, password);
                await _taskTrackerLogic.EditAccount(account);
                await Authenticate(account.Login);
                return RedirectToAction("GetUserAccount", "Home");
            }

            return View(accountModel);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> EditUser(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var oldLogin = User.Identity.Name;
                var oldAccount = await _taskTrackerLogic.GetAccount(oldLogin);
                var name = userModel.Name;
                var phoneNumber = userModel.PhoneNumber;

                var user = new User(oldAccount.Id, name, phoneNumber);
                await _taskTrackerLogic.EditUser(user);
                return RedirectToAction("GetUserAccount", "Home");
            }

            return View(userModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
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
