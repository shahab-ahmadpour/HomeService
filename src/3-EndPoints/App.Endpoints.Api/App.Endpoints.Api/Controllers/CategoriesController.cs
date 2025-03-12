using App.Domain.Core.DTO.Categories;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;
        private readonly IHomeServiceAppService _homeServiceAppService;
        private readonly Serilog.ILogger _logger;

        public CategoriesController(
            ICategoryAppService categoryAppService,
            IHomeServiceAppService homeServiceAppService,
            Serilog.ILogger logger)
        {
            _categoryAppService = categoryAppService;
            _homeServiceAppService = homeServiceAppService;
            _logger = logger;
        }

        /// <summary>
        /// دریافت همه دسته‌بندی‌ها
        /// </summary>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>لیست تمام دسته‌بندی‌ها</returns>
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken cancellationToken)
        {
            _logger.Information("API: دریافت تمام دسته‌بندی‌ها");
            var categories = await _categoryAppService.GetAllAsync(cancellationToken);
            return Ok(categories);
        }

        /// <summary>
        /// دریافت دسته‌بندی با شناسه
        /// </summary>
        /// <param name="id">شناسه دسته‌بندی</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>دسته‌بندی با شناسه مشخص</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken cancellationToken)
        {
            _logger.Information("API: دریافت دسته‌بندی با شناسه {Id}", id);
            var category = await _categoryAppService.GetAsync(id, cancellationToken);
            if (category == null)
            {
                _logger.Warning("API: دسته‌بندی با شناسه {Id} یافت نشد", id);
                return NotFound();
            }
            return Ok(category);
        }

        /// <summary>
        /// دریافت تمام دسته‌بندی‌ها به همراه سرویس‌ها
        /// </summary>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>لیست دسته‌بندی‌ها به همراه سرویس‌های مرتبط</returns>
        [HttpGet("with-services")]
        [ApiKeyAuth] // اعمال فیلتر احراز هویت API Key
        public async Task<ActionResult<List<CategoryDto>>> GetAllWithServices(CancellationToken cancellationToken)
        {
            _logger.Information("API: دریافت تمام دسته‌بندی‌ها به همراه سرویس‌ها");
            var categoriesWithServices = await _categoryAppService.GetAllWithServicesAsync(cancellationToken);
            return Ok(categoriesWithServices);
        }
    }
}