using App.Domain.Core.DTO.Categories;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.CategoryAppServices
{
    public class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;

        public CategoryAppService(ICategoryService categoryService, ILogger logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Creating new category with Name: {Name}", dto.Name);
            var result = await _categoryService.CreateAsync(dto, cancellationToken);
            _logger.Information("AppService: CreateAsync returned: {Result}", result);
            return result;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Updating category with Id: {Id}", id);
            var result = await _categoryService.UpdateAsync(id, dto, cancellationToken);
            _logger.Information("AppService: UpdateAsync returned: {Result}", result);
            return result;
        }

        public async Task<CategoryDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Getting category with Id: {Id}", id);
            return await _categoryService.GetAsync(id, cancellationToken);
        }

        public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Getting all categories");
            return await _categoryService.GetAllAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Deleting category with Id: {Id}", id);
            var result = await _categoryService.DeleteAsync(id, cancellationToken);
            _logger.Information("AppService: DeleteAsync returned: {Result}", result);
            return result;
        }

        public async Task<List<CategoryDto>> GetAllWithServicesAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all categories with services");
            return await _categoryService.GetAllWithServicesAsync(cancellationToken);
        }
    }
}
