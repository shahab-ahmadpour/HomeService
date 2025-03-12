using App.Domain.Core.DTO.HomeServices;
using App.Domain.Core.DTO.SubHomeServices;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Endpoints.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Endpoints.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IHomeServiceAppService _homeServiceAppService;
        private readonly ISubHomeServiceAppService _subHomeServiceAppService;
        private readonly Serilog.ILogger _logger;

        public ServicesController(
            IHomeServiceAppService homeServiceAppService,
            ISubHomeServiceAppService subHomeServiceAppService,
            Serilog.ILogger logger)
        {
            _homeServiceAppService = homeServiceAppService;
            _subHomeServiceAppService = subHomeServiceAppService;
            _logger = logger;
        }

        /// <summary>
        /// دریافت همه سرویس‌های اصلی
        /// </summary>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>لیست تمام سرویس‌های اصلی</returns>
        [HttpGet]
        public async Task<ActionResult<List<HomeServiceListItemDto>>> GetAllHomeServices(CancellationToken cancellationToken)
        {
            _logger.Information("API: دریافت تمام سرویس‌های اصلی");
            var homeServices = await _homeServiceAppService.GetAllAsync(cancellationToken);
            return Ok(homeServices);
        }

        /// <summary>
        /// دریافت سرویس اصلی با شناسه
        /// </summary>
        /// <param name="id">شناسه سرویس اصلی</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>سرویس اصلی با شناسه مشخص</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeServiceDto>> GetHomeServiceById(int id, CancellationToken cancellationToken)
        {
            _logger.Information("API: دریافت سرویس اصلی با شناسه {Id}", id);
            var homeService = await _homeServiceAppService.GetAsync(id, cancellationToken);
            if (homeService == null)
            {
                _logger.Warning("API: سرویس اصلی با شناسه {Id} یافت نشد", id);
                return NotFound();
            }
            return Ok(homeService);
        }

        /// <summary>
        /// دریافت همه سرویس‌های اصلی به همراه زیرسرویس‌ها
        /// </summary>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>لیست سرویس‌های اصلی به همراه زیرسرویس‌های مرتبط</returns>
        [HttpGet("with-sub-services")]
        [ApiKeyAuth] // اعمال فیلتر احراز هویت API Key
        public async Task<ActionResult<List<HomeServiceDto>>> GetAllWithSubServices(CancellationToken cancellationToken)
        {
            _logger.Information("API: دریافت تمام سرویس‌های اصلی به همراه زیرسرویس‌ها");
            var homeServicesWithSubServices = await _homeServiceAppService.GetAllWithSubServicesAsync(cancellationToken);
            return Ok(homeServicesWithSubServices);
        }

        /// <summary>
        /// دریافت زیرسرویس‌های یک سرویس اصلی
        /// </summary>
        /// <param name="homeServiceId">شناسه سرویس اصلی</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>لیست زیرسرویس‌های سرویس اصلی</returns>
        [HttpGet("{homeServiceId}/sub-services")]
        public async Task<ActionResult<List<SubHomeServiceListItemDto>>> GetSubServicesByHomeServiceId(int homeServiceId, CancellationToken cancellationToken)
        {
            _logger.Information("API: دریافت زیرسرویس‌های سرویس اصلی با شناسه {HomeServiceId}", homeServiceId);
            var subServices = await _subHomeServiceAppService.GetSubHomeServicesByHomeServiceIdAsync(homeServiceId, cancellationToken);
            if (subServices == null || subServices.Count == 0)
            {
                _logger.Warning("API: هیچ زیرسرویسی برای سرویس اصلی با شناسه {HomeServiceId} یافت نشد", homeServiceId);
                return NotFound();
            }
            return Ok(subServices);
        }

        /// <summary>
        /// دریافت اطلاعات یک زیرسرویس
        /// </summary>
        /// <param name="id">شناسه زیرسرویس</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>اطلاعات زیرسرویس</returns>
        [HttpGet("sub-services/{id}")]
        public async Task<ActionResult<SubHomeServiceDto>> GetSubServiceById(int id, CancellationToken cancellationToken)
        {
            _logger.Information("API: دریافت زیرسرویس با شناسه {Id}", id);
            var subService = await _subHomeServiceAppService.GetAsync(id, cancellationToken);
            if (subService == null)
            {
                _logger.Warning("API: زیرسرویس با شناسه {Id} یافت نشد", id);
                return NotFound();
            }
            return Ok(subService);
        }
    }
}