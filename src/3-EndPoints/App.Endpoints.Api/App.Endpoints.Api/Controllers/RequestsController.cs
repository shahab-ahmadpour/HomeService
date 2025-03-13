using App.Domain.Core.DTO.Requests;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Endpoints.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Endpoints.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestAppService _requestAppService;
        private readonly Serilog.ILogger _logger;

        public RequestsController(
            IRequestAppService requestAppService,
            Serilog.ILogger logger)
        {
            _requestAppService = requestAppService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("API: دریافت تمام درخواست‌ها");
                var requests = await _requestAppService.GetAllAsync(cancellationToken);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت تمام درخواست‌ها: {Message}", ex.Message);
                return StatusCode(500, new { error = "خطای داخلی سرور", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("API: دریافت درخواست با شناسه {Id}", id);
                var request = await _requestAppService.GetAsync(id, cancellationToken);

                if (request == null)
                {
                    return NotFound(new { error = "درخواست یافت نشد" });
                }

                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت درخواست با شناسه {Id}: {Message}", id, ex.Message);
                return StatusCode(500, new { error = "خطای داخلی سرور", details = ex.Message });
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("API: دریافت درخواست‌های مشتری با شناسه {CustomerId}", customerId);
                var requests = await _requestAppService.GetRequestsByCustomerIdAsync(customerId, cancellationToken);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت درخواست‌های مشتری با شناسه {CustomerId}: {Message}", customerId, ex.Message);
                return StatusCode(500, new { error = "خطای داخلی سرور", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto model, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.Information("API: ایجاد درخواست جدید برای مشتری با شناسه {CustomerId}", model.CustomerId);
                var result = await _requestAppService.CreateRequestAsync(model, cancellationToken);

                if (result)
                {
                    return Ok(new { success = true, message = "درخواست با موفقیت ایجاد شد" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "خطا در ایجاد درخواست" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در ایجاد درخواست: {Message}", ex.Message);
                return StatusCode(500, new { error = "خطای داخلی سرور", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRequestDto model, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.Information("API: به‌روزرسانی درخواست با شناسه {Id}", id);
                var result = await _requestAppService.UpdateAsync(id, model, cancellationToken);

                if (result)
                {
                    return Ok(new { success = true, message = "درخواست با موفقیت به‌روزرسانی شد" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "خطا در به‌روزرسانی درخواست" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در به‌روزرسانی درخواست با شناسه {Id}: {Message}", id, ex.Message);
                return StatusCode(500, new { error = "خطای داخلی سرور", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("API: حذف درخواست با شناسه {Id}", id);
                var result = await _requestAppService.DeleteAsync(id, cancellationToken);

                if (result)
                {
                    return Ok(new { success = true, message = "درخواست با موفقیت حذف شد" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "خطا در حذف درخواست" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در حذف درخواست با شناسه {Id}: {Message}", id, ex.Message);
                return StatusCode(500, new { error = "خطای داخلی سرور", details = ex.Message });
            }
        }
    }
}