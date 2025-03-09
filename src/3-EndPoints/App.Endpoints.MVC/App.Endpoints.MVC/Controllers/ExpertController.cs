using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.DTO.Users.Experts;
using App.Domain.Core.Enums;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Users.Interfaces.IAppService;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace App.Endpoints.MVC.Controllers
{
    public class ExpertController : Controller
    {
        private readonly IExpertAppService _expertAppService;
        private readonly IProposalAppService _proposalAppService;
        private readonly IOrderAppService _orderAppService;
        private readonly IRequestAppService _requestAppService;
        private readonly Serilog.ILogger _logger;

        public ExpertController(
            IExpertAppService expertAppService,
            IProposalAppService proposalAppService,
            IOrderAppService orderAppService,
            IRequestAppService requestAppService,
            Serilog.ILogger logger)
        {
            _expertAppService = expertAppService;
            _proposalAppService = proposalAppService;
            _orderAppService = orderAppService;
            _requestAppService = requestAppService;
            _logger = logger;
        }

        private async Task<int?> GetExpertIdFromSession(CancellationToken cancellationToken)
        {
            var appUserId = HttpContext.Session.GetInt32("UserId");
            if (!appUserId.HasValue)
            {
                _logger.Warning("No UserId (AppUserId) in session, redirecting to Login");
                return null;
            }

            _logger.Information("Session UserId (AppUserId): {AppUserId}", appUserId.Value);

            try
            {
                var expertId = await _expertAppService.GetExpertIdByAppUserIdAsync(appUserId.Value, cancellationToken);
                if (expertId <= 0)
                {
                    _logger.Warning("Expert not found for AppUserId: {AppUserId}", appUserId.Value);
                    return null;
                }

                _logger.Information("Found Expert with ExpertId: {ExpertId} for AppUserId: {AppUserId}",
                    expertId, appUserId.Value);
                return expertId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting expert ID for AppUserId: {AppUserId}", appUserId.Value);
                return null;
            }
        }

        // GET: Expert/Dashboard
        public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var expertDto = await _expertAppService.GetByIdAsync(expertId.Value, cancellationToken);
            if (expertDto == null)
            {
                _logger.Warning("Expert not found for ExpertId: {ExpertId}", expertId.Value);
                return RedirectToAction("Login", "Account");
            }

            var proposals = await _proposalAppService.GetProposalsByExpertIdAsync(expertId.Value, cancellationToken);
            var orders = await _orderAppService.GetOrdersByExpertIdAsync(expertId.Value, cancellationToken);

            ViewBag.Proposals = proposals;
            ViewBag.Orders = orders;
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            _logger.Information("Dashboard loaded successfully for ExpertId: {ExpertId}", expertId.Value);
            return View(expertDto);
        }

        // GET: Expert/EditProfile
        public async Task<IActionResult> EditProfile(CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var editDto = await _expertAppService.GetEditExpertProfileAsync(expertId.Value, cancellationToken);
            if (editDto == null)
            {
                _logger.Warning("Expert not found for ExpertId: {ExpertId}", expertId.Value);
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(editDto);
        }

        // POST: Expert/EditProfile
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditExpertDto model, CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var expertDto = await _expertAppService.GetByIdAsync(expertId.Value, cancellationToken);
            if (expertDto == null)
            {
                _logger.Warning("Expert not found for ExpertId: {ExpertId}", expertId.Value);
                return RedirectToAction("Login", "Account");
            }

            model.AppUserId = expertDto.AppUserId;
            _logger.Information("Received model for update: FirstName={FirstName}, LastName={LastName}, PhoneNumber={PhoneNumber}, ProfilePicture={ProfilePicture}",
                model.FirstName, model.LastName, model.PhoneNumber, model.ProfilePicture);

            ModelState.Clear();

            if (!ModelState.IsValid)
            {
                _logger.Warning("ModelState is invalid after clear for ExpertId: {ExpertId}", expertId.Value);
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.Warning("Validation error: {Error}", error.ErrorMessage);
                }
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                return View(model);
            }

            if (model.ProfilePictureFile != null)
            {
                try
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfilePictureFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePictureFile.CopyToAsync(stream);
                    }
                    model.ProfilePicture = $"/uploads/{fileName}";
                    _logger.Information("New profile picture uploaded: {ProfilePicture}", model.ProfilePicture);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to upload profile picture for ExpertId: {ExpertId}", expertId.Value);
                    ModelState.AddModelError("ProfilePictureFile", "خطا در آپلود عکس پروفایل.");
                    ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                    return View(model);
                }
            }
            else
            {
                model.ProfilePicture = expertDto.ProfilePicture;
                _logger.Information("No new profile picture, keeping existing: {ProfilePicture}", model.ProfilePicture);
            }

            var result = await _expertAppService.UpdateExpertProfileAsync(model, cancellationToken);
            if (result)
            {
                _logger.Information("Profile updated successfully for ExpertId: {ExpertId}", expertId.Value);
                TempData["SuccessMessage"] = "پروفایل شما با موفقیت به‌روزرسانی شد!";
                return RedirectToAction("Dashboard");
            }
            _logger.Warning("Failed to update profile for ExpertId: {ExpertId}", expertId.Value);
            ModelState.AddModelError("", "خطا در به‌روزرسانی پروفایل. لطفاً دوباره تلاش کنید.");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(model);
        }

        // GET: Expert/Proposals
        public async Task<IActionResult> Proposals(CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var proposals = await _proposalAppService.GetProposalsByExpertIdAsync(expertId.Value, cancellationToken);
            _logger.Information("Proposals loaded successfully for ExpertId: {ExpertId}, Count: {ProposalCount}",
                expertId.Value, proposals?.Count ?? 0);

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(proposals);
        }

        // GET: Expert/Orders
        public async Task<IActionResult> Orders(CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _orderAppService.GetOrdersByExpertIdAsync(expertId.Value, cancellationToken);
            _logger.Information("Orders loaded successfully for ExpertId: {ExpertId}, Count: {OrderCount}",
                expertId.Value, orders?.Count ?? 0);

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(orders);
        }

        // GET: Expert/OrderDetails
        public async Task<IActionResult> OrderDetails(int orderId, CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                _logger.Warning("No ExpertId in session, redirecting to Login");
                return RedirectToAction("Login", "Account");
            }

            var order = await _orderAppService.GetAsync(orderId, cancellationToken);
            if (order == null)
            {
                _logger.Warning("Order not found for OrderId: {OrderId}", orderId);
                TempData["ErrorMessage"] = "سفارش یافت نشد.";
                return RedirectToAction("Orders");
            }

            if (order.ExpertId != expertId.Value)
            {
                _logger.Warning("Order {OrderId} does not belong to ExpertId: {ExpertId}", orderId, expertId.Value);
                TempData["ErrorMessage"] = "شما دسترسی به این سفارش ندارید.";
                return RedirectToAction("Orders");
            }

            _logger.Information("Order details loaded successfully for OrderId: {OrderId}, ExpertId: {ExpertId}",
                orderId, expertId.Value);
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(order);
        }

        // GET: Expert/AvailableRequests
        public async Task<IActionResult> AvailableRequests(CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var requests = await _requestAppService.GetAvailableRequestsForExpertAsync(expertId.Value, cancellationToken);
            _logger.Information("Available requests loaded successfully for ExpertId: {ExpertId}, Count: {RequestCount}",
                expertId.Value, requests?.Count ?? 0);

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(requests);
        }

        // GET: Expert/CreateProposal
        public async Task<IActionResult> CreateProposal(int requestId, CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var request = await _requestAppService.GetAsync(requestId, cancellationToken);
            if (request == null)
            {
                _logger.Warning("Request not found for RequestId: {RequestId}", requestId);
                TempData["ErrorMessage"] = "درخواست یافت نشد.";
                return RedirectToAction("AvailableRequests");
            }

            var model = new CreateProposalDto
            {
                ExpertId = expertId.Value,
                RequestId = requestId,
                ExecutionDate = DateTime.Now.AddDays(3)
            };

            ViewBag.Request = request;
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProposal(CreateProposalDto model, CancellationToken cancellationToken)
        {
            var expertId = await GetExpertIdFromSession(cancellationToken);
            if (!expertId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                var request = await _requestAppService.GetAsync(model.RequestId, cancellationToken);
                ViewBag.Request = request;
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                return View(model);
            }

            model.ExpertId = expertId.Value;
            model.Status = ProposalStatus.Pending;

            var result = await _proposalAppService.CreateProposalAsync(model, cancellationToken);
            if (result)
            {
                _logger.Information("Proposal created successfully for ExpertId: {ExpertId}, RequestId: {RequestId}",
                    expertId.Value, model.RequestId);
                TempData["SuccessMessage"] = "پیشنهاد شما با موفقیت ثبت شد!";
                return RedirectToAction("Proposals");
            }
            else
            {
                _logger.Warning("Failed to create proposal for ExpertId: {ExpertId}, RequestId: {RequestId}",
                    expertId.Value, model.RequestId);
                TempData["ErrorMessage"] = "خطا در ثبت پیشنهاد.";
                return RedirectToAction("AvailableRequests");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _logger.Information("User logged out, clearing session for AppUserId: {AppUserId}", HttpContext.Session.GetInt32("UserId"));
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}