using App.Domain.Core.Services.Interfaces.IAppService;
using App.Endpoints.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        public async Task<IActionResult> GetAllHomeServices(CancellationToken cancellationToken)
        {
            try
            {
                var homeServices = await _homeServiceAppService.GetAllAsync(cancellationToken);
                return Ok(homeServices);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت سرویس‌های اصلی");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHomeServiceById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var homeService = await _homeServiceAppService.GetAsync(id, cancellationToken);

                if (homeService == null)
                {
                    return NotFound("سرویس اصلی یافت نشد");
                }

                return Ok(homeService);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت سرویس اصلی");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpGet("with-sub-services")]
        [ApiKeyAuth]
        public async Task<IActionResult> GetAllWithSubServices(CancellationToken cancellationToken)
        {
            try
            {
                var homeServicesWithSubServices = await _homeServiceAppService.GetAllWithSubServicesAsync(cancellationToken);
                return Ok(homeServicesWithSubServices);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت سرویس‌های اصلی با زیرسرویس‌ها");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpGet("{homeServiceId}/sub-services")]
        public async Task<IActionResult> GetSubServicesByHomeServiceId(int homeServiceId, CancellationToken cancellationToken)
        {
            try
            {
                var subServices = await _subHomeServiceAppService.GetSubHomeServicesByHomeServiceIdAsync(homeServiceId, cancellationToken);

                if (subServices == null || subServices.Count == 0)
                {
                    return NotFound("زیرسرویسی برای سرویس اصلی مشخص شده یافت نشد");
                }

                return Ok(subServices);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت زیرسرویس‌ها");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpGet("sub-services/{id}")]
        public async Task<IActionResult> GetSubServiceById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var subService = await _subHomeServiceAppService.GetAsync(id, cancellationToken);

                if (subService == null)
                {
                    return NotFound("زیرسرویس یافت نشد");
                }

                return Ok(subService);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت زیرسرویس");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }
    }
}