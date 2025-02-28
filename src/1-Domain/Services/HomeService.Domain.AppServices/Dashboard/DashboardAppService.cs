using App.Domain.Core.DTO.Dashboard;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IService;
using App.Domain.Core.Users.Interfaces.IAppService;
using App.Domain.Core.Users.Interfaces.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.Dashboard
{
    public class DashboardAppService : IDashboardAppService
    {
        private readonly IAdminUserService _userService;
        private readonly IOrderService _orderService;
        private readonly ISubHomeServiceService _serviceService;
        private readonly IReviewService _reviewService;

        public DashboardAppService(
            IAdminUserService userService,
            IOrderService orderService,
            ISubHomeServiceService serviceService,
            IReviewService reviewService)
        {
            _userService = userService;
            _orderService = orderService;
            _serviceService = serviceService;
            _reviewService = reviewService;
        }

        public DashboardDto GetDashboardStats()
        {
            return new DashboardDto
            {
                NewOrders = _orderService.GetAllOrders().Count,
                RegisteredUsers = _userService.GetAllUsers().Count,
                Services = _serviceService.GetAllServices().Count,
                Reviews = _reviewService.GetAllReviews().Count
            };
        }
    }
}
