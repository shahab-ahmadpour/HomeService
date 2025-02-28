using App.Domain.Core.DTO.Users.AppUsers;
using App.Domain.Core.Enums;
using App.Domain.Core.Users.Entities;
using App.Domain.Core.Users.Interfaces.IAppService;
using App.Endpoints.MVC.Models.Account;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace App.Endpoints.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAppService _userAppService;
        private readonly Serilog.ILogger _logger;

        public AccountController(IUserAppService userAppService, Serilog.ILogger logger)
        {
            _userAppService = userAppService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            _logger.Information("Displaying the registration form.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
        {
            _logger.Information("Received registration request for email: {Email}", model.Email);

            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid registration model for email: {Email}", model.Email);
                return View(model);
            }

            var dto = new CreateAppUserDto
            {
                Email = model.Email,
                Password = model.Password,
                Role = Enum.TryParse<UserRole>(model.Role, out var parsedRole) ? parsedRole : UserRole.Customer,
                IsEnabled = true,
                IsConfirmed = false
            };

            var result = await _userAppService.RegisterAsync(dto, model.Password, cancellationToken);

            if (result.Succeeded)
            {
                _logger.Information("User with email {Email} registered successfully.", model.Email);
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.Warning("Failed to register user {Email}. Error: {Error}", model.Email, error.Description);
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            _logger.Information("Displaying the login form.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
        {
            _logger.Information("Attempting to log in user with email: {Email}", model.Email);

            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid login model for email: {Email}", model.Email);
                return View(model);
            }

            var result = await _userAppService.LoginAsync(model.Email, model.Password, model.RememberMe);

            if (result.Succeeded)
            {
                var user = await _userAppService.GetUserByEmailAsync(model.Email, cancellationToken);
                if (user != null)
                {
                    if (!user.IsEnabled)
                    {
                        _logger.Warning("Login failed for email {Email}: User is disabled", model.Email);
                        ModelState.AddModelError(string.Empty, "حساب کاربری شما غیرفعال است. لطفاً با پشتیبانی تماس بگیرید.");
                        return View(model);
                    }

                    if (!user.IsConfirmed)
                    {
                        _logger.Warning("Login failed for email {Email}: User is not confirmed", model.Email);
                        ModelState.AddModelError(string.Empty, "حساب کاربری شما هنوز تأیید نشده است. لطفاً منتظر تأیید توسط مدیریت باشید.");
                        return View(model);
                    }

                    _logger.Information("User with email {Email} logged in successfully. Role: {Role}", model.Email, user.Role);
                    HttpContext.Session.SetInt32("AppUserId", user.Id);

                    return user.Role switch
                    {
                        UserRole.Admin => RedirectToAction("Index", "Dashboard", new { area = "Admin" }),
                        UserRole.Customer => RedirectToAction("Dashboard", "Customer"),
                        UserRole.Expert => RedirectToAction("Index", "ExpertDashboard"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }
            }

            _logger.Warning("Login failed for email {Email}: Invalid credentials", model.Email);
            ModelState.AddModelError(string.Empty, "ایمیل یا رمز عبور اشتباه است.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.Information("Attempting to log out the current user.");
            await _userAppService.LogoutAsync();
            HttpContext.Session.Clear();
            _logger.Information("User logged out successfully.");
            return RedirectToAction("Index", "Home");
        }
    }
}
