using App.Domain.Core.DTO.Categories;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Dapper.Services
{
    public class CategoryDapperRepository : DapperRepositoryBase, ICategoryRepository
    {
        public CategoryDapperRepository(string connectionString, ILogger logger)
            : base(connectionString, logger)
        {
        }

        public async Task<bool> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Creating new category: {dto.Name}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        INSERT INTO Categories (Name, Description, ImagePath, IsActive)
                        VALUES (@Name, @Description, @ImagePath, 1)";

                    var parameters = new
                    {
                        dto.Name,
                        dto.Description,
                        ImagePath = dto.ImagePath ?? "/images/Categories/default.jpg"
                    };

                    var result = await connection.ExecuteAsync(query, parameters);
                    _logger.Information("Successfully created category: {Name}", dto.Name);
                    return result > 0;
                }
            });
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Updating category with ID: {id}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var existsQuery = "SELECT COUNT(1) FROM Categories WHERE Id = @Id";
                    var exists = await connection.ExecuteScalarAsync<int>(existsQuery, new { Id = id });

                    if (exists == 0)
                    {
                        _logger.Warning("Category with ID: {Id} not found", id);
                        return false;
                    }

                    var updateQuery = @"
                        UPDATE Categories 
                        SET Name = @Name, 
                            Description = @Description, 
                            ImagePath = @ImagePath, 
                            IsActive = @IsActive
                        WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        dto.Name,
                        dto.Description,
                        dto.ImagePath,
                        dto.IsActive
                    };

                    var result = await connection.ExecuteAsync(updateQuery, parameters);
                    _logger.Information("Successfully updated category with ID: {Id}", id);
                    return result > 0;
                }
            });
        }

        public async Task<CategoryDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Fetching category with ID: {id}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT Id, Name, Description, ImagePath, IsActive 
                        FROM Categories
                        WHERE Id = @Id";

                    var parameters = new { Id = id };
                    var category = await connection.QueryFirstOrDefaultAsync<CategoryDto>(query, parameters);

                    if (category == null)
                    {
                        _logger.Warning("Category with ID: {Id} not found", id);
                    }
                    else
                    {
                        category.ImagePath = category.ImagePath?.Replace("\\", "/");
                        _logger.Information("Fetched category with ID: {Id}, ImagePath: {ImagePath}", id, category.ImagePath);
                    }

                    return category;
                }
            });
        }

        public async Task<List<CategoryListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync("Fetching all categories", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT Id, Name
                        FROM Categories
                        WHERE IsActive = 1";

                    var categories = await connection.QueryAsync<CategoryListItemDto>(query);
                    var categoryList = categories.ToList();

                    _logger.Information("Fetched {Count} categories", categoryList.Count);
                    return categoryList;
                }
            });
        }

        public async Task<List<CategoryDto>> GetAllDetailedAsync(CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync("Fetching all detailed categories", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT Id, Name, Description, ImagePath, IsActive
                        FROM Categories";

                    var categories = await connection.QueryAsync<CategoryDto>(query);
                    var categoryList = categories.ToList();

                    foreach (var category in categoryList)
                    {
                        category.ImagePath = category.ImagePath?.Replace("\\", "/");
                    }

                    _logger.Information("Fetched {Count} detailed categories", categoryList.Count);
                    return categoryList;
                }
            });
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Deleting (deactivating) category with ID: {id}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var existsQuery = "SELECT COUNT(1) FROM Categories WHERE Id = @Id";
                    var exists = await connection.ExecuteScalarAsync<int>(existsQuery, new { Id = id });

                    if (exists == 0)
                    {
                        _logger.Warning("Category with ID: {Id} not found for deletion", id);
                        return false;
                    }

                    var updateQuery = @"
                        UPDATE Categories 
                        SET IsActive = 0
                        WHERE Id = @Id";

                    var parameters = new { Id = id };
                    var result = await connection.ExecuteAsync(updateQuery, parameters);

                    _logger.Information("Successfully deactivated category with ID: {Id}", id);
                    return result > 0;
                }
            });
        }

        public async Task<List<Category>> GetAllWithServicesAsync(CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync("Fetching all categories with services", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {

                    var categories = await connection.QueryAsync<Category>("SELECT * FROM Categories");
                    var categoryList = categories.ToList();

                    foreach (var category in categoryList)
                    {
                        var homeServicesQuery = "SELECT * FROM HomeServices WHERE CategoryId = @CategoryId";
                        var homeServices = await connection.QueryAsync<HomeService>(homeServicesQuery, new { CategoryId = category.Id });
                        category.HomeServices = homeServices.ToList();

                        foreach (var homeService in category.HomeServices)
                        {
                            var subServicesQuery = "SELECT * FROM SubHomeServices WHERE HomeServiceId = @HomeServiceId AND IsActive = 1";
                            var subServices = await connection.QueryAsync<SubHomeService>(subServicesQuery, new { HomeServiceId = homeService.Id });
                            homeService.SubHomeServices = subServices.ToList();
                        }
                    }

                    return categoryList;
                }
            });
        }
    }
}