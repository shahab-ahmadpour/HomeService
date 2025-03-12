using App.Domain.Core.DTO.Requests;
using App.Domain.Core.Enums;
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

        /// <summary>
        /// دریافت تمام درخواست‌ها
        /// </summary>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>لیست تمام درخواست‌ها</returns>
        [HttpGet]
        public async Task<ActionResult<List<RequestDto>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("API: دریافت تمام درخواست‌ها");
                var requests = await _requestAppService.GetAllAsync(cancellationToken);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API: خطا در دریافت تمام درخواست‌ها");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        /// <summary>
        /// دریافت درخواست با شناسه
        /// </summary>
        /// <param name="id">شناسه درخواست</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>درخواست با شناسه مشخص</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDto>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("API: دریافت درخواست با شناسه {Id}", id);
                var request = await _requestAppService.GetAsync(id, cancellationToken);
                if (request == null)
                {
                    _logger.Warning("API: درخواست با شناسه {Id} یافت نشد", id);
                    return NotFound();
                }
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API: خطا در دریافت درخواست با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        /// <summary>
        /// دریافت درخواست‌های مربوط به یک مشتری
        /// </summary>
        /// <param name="customerId">شناسه مشتری</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>لیست درخواست‌های مشتری</returns>
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<RequestDto>>> GetByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("API: دریافت درخواست‌های مشتری با شناسه {CustomerId}", customerId);
                var requests = await _requestAppService.GetRequestsByCustomerIdAsync(customerId, cancellationToken);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API: خطا در دریافت درخواست‌های مشتری با شناسه {CustomerId}", customerId);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        /// <summary>
        /// دریافت درخواست‌های در دسترس برای کارشناس
        /// </summary>
        /// <param name="expertId">شناسه کارشناس</param>
        /// <param name="expertState">استان کارشناس</param>
        /// <param name="subHomeServiceIds">شناسه زیرسرویس‌هایی که کارشناس ارائه می‌دهد</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>لیست درخواست‌های در دسترس برای کارشناس</returns>
        [HttpGet("available-for-expert/{expertId}")]
        public async Task<ActionResult<List<RequestDto>>> GetAvailableForExpert(
            int expertId,
            [FromQuery] string expertState,
            [FromQuery] List<int> subHomeServiceIds,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("API: دریافت درخواست‌های در دسترس برای کارشناس با شناسه {ExpertId}", expertId);
                if (string.IsNullOrEmpty(expertState))
                {
                    return BadRequest("استان کارشناس الزامی است");
                }

                if (subHomeServiceIds == null || subHomeServiceIds.Count == 0)
                {
                    return BadRequest("حداقل یک زیرسرویس باید مشخص شود");
                }

                var requests = await _requestAppService.GetAvailableRequestsForExpertAsync(
                    expertId,
                    expertState,
                    subHomeServiceIds,
                    cancellationToken);

                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API: خطا در دریافت درخواست‌های در دسترس برای کارشناس با شناسه {ExpertId}", expertId);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        /// <summary>
        /// ایجاد درخواست جدید
        /// </summary>
        /// <param name="model">اطلاعات درخواست جدید</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>نتیجه عملیات</returns>
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
                    _logger.Warning("API: ایجاد درخواست برای مشتری با شناسه {CustomerId} ناموفق بود", model.CustomerId);
                    return BadRequest(new { success = false, message = "خطا در ایجاد درخواست" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API: خطا در ایجاد درخواست برای مشتری با شناسه {CustomerId}", model.CustomerId);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        /// <summary>
        /// به‌روزرسانی وضعیت درخواست
        /// </summary>
        /// <param name="id">شناسه درخواست</param>
        /// <param name="model">اطلاعات به‌روزرسانی</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>نتیجه عملیات</returns>
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
                    _logger.Warning("API: به‌روزرسانی درخواست با شناسه {Id} ناموفق بود", id);
                    return BadRequest(new { success = false, message = "خطا در به‌روزرسانی درخواست" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API: خطا در به‌روزرسانی درخواست با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        /// <summary>
        /// حذف درخواست
        /// </summary>
        /// <param name="id">شناسه درخواست</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>نتیجه عملیات</returns>
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
                    _logger.Warning("API: حذف درخواست با شناسه {Id} ناموفق بود", id);
                    return BadRequest(new { success = false, message = "خطا در حذف درخواست" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API: خطا در حذف درخواست با شناسه {Id}", id);
                return StatusCode(500, "خطای داخلی سرور");
            }
        }
    }
}