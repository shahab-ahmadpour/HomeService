using App.Domain.Core.DTO.Categories;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;
        private readonly Serilog.ILogger _logger;

        public CategoriesController(
            ICategoryAppService categoryAppService,
            Serilog.ILogger logger)
        {
            _categoryAppService = categoryAppService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryAppService.GetAllAsync(cancellationToken);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت دسته‌بندی‌ها");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _categoryAppService.GetAsync(id, cancellationToken);

                if (category == null)
                {
                    return NotFound("دسته‌بندی یافت نشد");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت دسته‌بندی");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }

        [HttpGet("with-services")]
        [ApiKeyAuth]
        public async Task<IActionResult> GetAllWithServices(CancellationToken cancellationToken)
        {
            try
            {
                var categoriesWithServices = await _categoryAppService.GetAllWithServicesAsync(cancellationToken);
                return Ok(categoriesWithServices);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در دریافت دسته‌بندی‌ها با سرویس‌ها");
                return StatusCode(500, "خطای داخلی سرور");
            }
        }
    }
}