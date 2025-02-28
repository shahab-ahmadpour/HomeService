using App.Domain.Core.DTO.Categories;
using App.Domain.Core.DTO.HomeServices;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.HomeServiceAppServices
{
    public class HomeServiceAppService : IHomeServiceAppService
    {
        private readonly IHomeServiceService _homeServiceService;
        private readonly ICategoryService _categoryService; 
        private readonly ILogger _logger;

        public HomeServiceAppService(
            IHomeServiceService homeServiceService,
            ICategoryService categoryService,
            ILogger logger)
        {
            _homeServiceService = homeServiceService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateHomeServiceDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Creating HomeService with Name: {Name}", dto.Name);
            var result = await _homeServiceService.CreateAsync(dto, cancellationToken);
            _logger.Information("AppService: CreateAsync returned: {Result}", result);
            return result;
        }

        public async Task<bool> UpdateAsync(int id, UpdateHomeServiceDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Updating HomeService with Id: {Id}", id);
            var result = await _homeServiceService.UpdateAsync(id, dto, cancellationToken);
            _logger.Information("AppService: UpdateAsync returned: {Result}", result);
            return result;
        }

        public async Task<HomeServiceDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Getting HomeService with Id: {Id}", id);
            return await _homeServiceService.GetAsync(id, cancellationToken);
        }

        public async Task<List<HomeServiceListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Getting all HomeServices");
            return await _homeServiceService.GetAllAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Deleting HomeService with Id: {Id}", id);
            var result = await _homeServiceService.DeleteAsync(id, cancellationToken);
            _logger.Information("AppService: DeleteAsync returned: {Result}", result);
            return result;
        }

        public async Task<UpdateHomeServiceDto> GetHomeServiceForEditAsync(int id, CancellationToken cancellationToken)
        {
            var homeService = await _homeServiceService.GetAsync(id, cancellationToken);
            if (homeService == null)
            {
                _logger.Warning("HomeService with Id: {Id} not found.", id);
                return null;
            }

            var dto = new UpdateHomeServiceDto
            {
                Id = homeService.Id,
                Name = homeService.Name,
                Description = homeService.Description,
                ImagePath = homeService.ImagePath.StartsWith("/") ? homeService.ImagePath : "/" + homeService.ImagePath
            };

            return dto;
        }
        public async Task<List<HomeServiceDto>> GetAllWithSubServicesAsync(CancellationToken cancellationToken)
        {
            return await _homeServiceService.GetAllWithSubServicesAsync(cancellationToken);
        }
        public async Task<List<CategoryListItemDto>> GetAllCategoriesForDropdownAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all categories for dropdown");
            var categories = await _categoryService.GetAllForDropdownAsync(cancellationToken);
            _logger.Information("AppService: Fetched {Count} categories for dropdown", categories?.Count ?? 0);
            return categories ?? new List<CategoryListItemDto>();
        }
    }
}
