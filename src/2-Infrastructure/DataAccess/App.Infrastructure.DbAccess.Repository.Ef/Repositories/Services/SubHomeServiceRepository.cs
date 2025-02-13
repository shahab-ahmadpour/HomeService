using App.Domain.Core._ِDTO.SubHomeServices;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Services
{
    public class SubHomeServiceRepository : ISubHomeServiceRepository
    {
        private readonly AppDbContext _dbContext;

        public SubHomeServiceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(CreateSubHomeServiceDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var subHomeService = new SubHomeService
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ImagePath = dto.ImagePath,
                    BasePrice = dto.BasePrice,
                    HomeServiceId = dto.HomeServiceId,
                    IsActive = true
                };

                await _dbContext.SubHomeServices.AddAsync(subHomeService, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateAsync(int id, UpdateSubHomeServiceDto dto, CancellationToken cancellationToken)
        {
            var subHomeService = await _dbContext.SubHomeServices.FindAsync(id);
            if (subHomeService == null) return false;

            subHomeService.Name = dto.Name;
            subHomeService.Description = dto.Description;
            subHomeService.ImagePath = dto.ImagePath;
            subHomeService.BasePrice = dto.BasePrice;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<SubHomeServiceDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var subHomeService = await _dbContext.SubHomeServices
                .Where(s => s.Id == id && s.IsActive)
                .Select(s => new SubHomeServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ImagePath = s.ImagePath,
                    BasePrice = s.BasePrice,
                    HomeServiceId = s.HomeServiceId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return subHomeService;
        }

        public async Task<List<SubHomeServiceListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var subHomeServices = await _dbContext.SubHomeServices
                .Where(s => s.IsActive)
                .Select(s => new SubHomeServiceListItemDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync(cancellationToken);

            return subHomeServices;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var subHomeService = await _dbContext.SubHomeServices.FindAsync(id);
            if (subHomeService == null) return false;

            subHomeService.IsActive = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
