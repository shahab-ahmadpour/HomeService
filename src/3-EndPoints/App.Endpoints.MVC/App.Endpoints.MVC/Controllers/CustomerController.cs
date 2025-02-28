using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.DTO.Requests;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Users.Interfaces.IAppService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace App.Endpoints.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerAppService _customerAppService;
        private readonly IOrderAppService _orderAppService;
        private readonly IRequestAppService _requestAppService;
        private readonly ISubHomeServiceAppService _subHomeServiceAppService;
        private readonly IProposalAppService _proposalAppService;
        private readonly Serilog.ILogger _logger;

        public CustomerController(
            ICustomerAppService customerAppService,
            IOrderAppService orderAppService,
            IRequestAppService requestAppService,
            ISubHomeServiceAppService subHomeServiceAppService,
            IProposalAppService proposalAppService,
            Serilog.ILogger logger)
        {
            _customerAppService = customerAppService;
            _orderAppService = orderAppService;
            _requestAppService = requestAppService;
            _subHomeServiceAppService = subHomeServiceAppService;
            _proposalAppService = proposalAppService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var subHomeServices = await _subHomeServiceAppService.GetSubHomeServicesAsync(cancellationToken);
            ViewBag.SubHomeServices = subHomeServices;

            var model = new CreateRequestDto
            {
                CustomerId = appUserId.Value
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestDto model, CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                var subHomeServices = await _subHomeServiceAppService.GetSubHomeServicesAsync(cancellationToken);
                ViewBag.SubHomeServices = subHomeServices;
                ModelState.Clear(); // پاک کردن خطاها
                return View(new CreateRequestDto { CustomerId = appUserId.Value }); // لود مدل جدید
            }

            model.CustomerId = appUserId.Value;
            var result = await _requestAppService.CreateRequestAsync(model, cancellationToken);
            if (result)
            {
                _logger.Information("Request created successfully for CustomerId: {CustomerId}", appUserId.Value);
                return RedirectToAction("Dashboard");
            }
            else
            {
                _logger.Warning("Failed to create request for CustomerId: {CustomerId}", appUserId.Value);
                var subHomeServices = await _subHomeServiceAppService.GetSubHomeServicesAsync(cancellationToken);
                ViewBag.SubHomeServices = subHomeServices;
                ModelState.Clear(); // پاک کردن خطاها
                return View(new CreateRequestDto { CustomerId = appUserId.Value }); // لود مدل جدید
            }
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                _logger.Warning("User not logged in, redirecting to Login for AppUserId: {AppUserId}", appUserId);
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetByAppUserIdAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            _logger.Information("Dashboard loaded successfully for CustomerId: {CustomerId}", customer.Id);
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                _logger.Warning("User not logged in, redirecting to Login for AppUserId: {AppUserId}", appUserId);
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetByAppUserIdAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            var editDto = new EditCustomerDto
            {
                AppUserId = customer.AppUserId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                ProfilePicture = customer.ProfilePicture
            };

            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditCustomerDto model, CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            model.AppUserId = appUserId.Value;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _customerAppService.UpdateCustomerProfileAsync(model, cancellationToken);

            if (result)
            {
                _logger.Information("Profile updated successfully for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Dashboard");
            }
            else
            {
                _logger.Warning("Failed to update profile for AppUserId: {AppUserId}", appUserId.Value);
                ModelState.AddModelError("", "خطا در به‌روزرسانی پروفایل.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Orders(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                _logger.Warning("User not logged in, redirecting to Login for AppUserId: {AppUserId}", appUserId);
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetByAppUserIdAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            var orders = await _customerAppService.GetOrdersByCustomerIdAsync(customer.Id, cancellationToken);
            _logger.Information("Orders loaded successfully for CustomerId: {CustomerId}, Count: {OrderCount}", customer.Id, orders.Count);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Requests(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                _logger.Warning("User not logged in, redirecting to Login for AppUserId: {AppUserId}", appUserId);
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetByAppUserIdAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            var requests = await _requestAppService.GetRequestsByCustomerIdAsync(customer.Id, cancellationToken); // فرض می‌کنیم این متد وجود داره
            _logger.Information("Requests loaded successfully for CustomerId: {CustomerId}, Count: {RequestCount}", customer.Id, requests?.Count ?? 0);
            return View(requests);
        }

        [HttpGet]
        public async Task<IActionResult> Proposals(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                _logger.Warning("User not logged in, redirecting to Login for AppUserId: {AppUserId}", appUserId);
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetByAppUserIdAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            var proposals = await _customerAppService.GetProposalsByCustomerIdAsync(customer.Id, cancellationToken);
            _logger.Information("Proposals loaded successfully for CustomerId: {CustomerId}, Count: {ProposalCount}", customer.Id, proposals.Count);
            return View(proposals);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _logger.Information("User logged out, clearing session.");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}