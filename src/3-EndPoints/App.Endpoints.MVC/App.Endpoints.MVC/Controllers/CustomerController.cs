using App.Domain.Core.DTO.Requests;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.Locations.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Users.Interfaces.IAppService;
using App.Endpoints.MVC.Models;
using HomeService.Domain.AppServices.LocationAppServices;
using HomeService.Domain.AppServices.RequestAppServices;
using HomeService.Domain.AppServices.SubHomeSerAppServices;
using Microsoft.AspNetCore.Mvc;

namespace App.Endpoints.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerAppService _customerAppService;
        private readonly IOrderAppService _orderAppService;
        private readonly Serilog.ILogger _logger;

        public CustomerController(ICustomerAppService customerAppService, IOrderAppService orderAppService, Serilog.ILogger logger)
        {
            _customerAppService = customerAppService;
            _orderAppService = orderAppService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetByAppUserIdAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetEditCustomerProfileAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            return View(customer);
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
                _logger.Warning("Invalid model state for EditProfile with AppUserId: {AppUserId}", appUserId.Value);
                return View(model);
            }

            var result = await _customerAppService.UpdateCustomerProfileAsync(model, cancellationToken);
            if (!result)
            {
                _logger.Warning("Failed to update profile for AppUserId: {AppUserId}", appUserId.Value);
                return View(model);
            }

            ModelState.Clear();
            var updatedCustomer = await _customerAppService.GetEditCustomerProfileAsync(appUserId.Value, cancellationToken);
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Orders(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetByAppUserIdAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            var orders = await _customerAppService.GetOrdersByCustomerIdAsync(customer.Id, cancellationToken);
            _logger.Information("Orders retrieved for CustomerId {CustomerId}: {OrdersCount}", customer.Id, orders.Count);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Proposals(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("AppUserId");
            if (!appUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _customerAppService.GetByAppUserIdAsync(appUserId.Value, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId.Value);
                return RedirectToAction("Login", "Account");
            }

            var proposals = await _customerAppService.GetProposalsByCustomerIdAsync(customer.Id, cancellationToken);
            return View(proposals);
        }
    }
}
