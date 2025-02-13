using App.Domain.Core._ِDTO.Categories;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var category = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ImagePath = dto.ImagePath,
                    IsActive = true
                };

                await _dbContext.Categories.AddAsync(category, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null) return false;

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ImagePath = dto.ImagePath;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<CategoryDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == id && c.IsActive)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImagePath = c.ImagePath
                })
                .FirstOrDefaultAsync(cancellationToken);

            return category;
        }

        public async Task<List<CategoryListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var categories = await _dbContext.Categories
                .Where(c => c.IsActive)
                .Select(c => new CategoryListItemDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync(cancellationToken);

            return categories;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null) return false;

            category.IsActive = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }


}
