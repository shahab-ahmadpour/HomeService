using App.Domain.Core._ِDTO.HomeServices;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Services
{
    public class HomeServiceRepository : IHomeServiceRepository
    {
        private readonly AppDbContext _dbContext;

        public HomeServiceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(CreateHomeServiceDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var homeService = new HomeService
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ImagePath = dto.ImagePath,
                    CategoryId = dto.CategoryId,
                    IsActive = true
                };

                await _dbContext.HomeServices.AddAsync(homeService, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateAsync(int id, UpdateHomeServiceDto dto, CancellationToken cancellationToken)
        {
            var homeService = await _dbContext.HomeServices.FindAsync(id);
            if (homeService == null) return false;

            homeService.Name = dto.Name;
            homeService.Description = dto.Description;
            homeService.ImagePath = dto.ImagePath;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<HomeServiceDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var homeService = await _dbContext.HomeServices
                .Where(h => h.Id == id && h.IsActive)
                .Select(h => new HomeServiceDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    ImagePath = h.ImagePath,
                    CategoryId = h.CategoryId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return homeService;
        }

        public async Task<List<HomeServiceListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var homeServices = await _dbContext.HomeServices
                .Where(h => h.IsActive)
                .Select(h => new HomeServiceListItemDto
                {
                    Id = h.Id,
                    Name = h.Name
                })
                .ToListAsync(cancellationToken);

            return homeServices;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var homeService = await _dbContext.HomeServices.FindAsync(id);
            if (homeService == null) return false;

            homeService.IsActive = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
