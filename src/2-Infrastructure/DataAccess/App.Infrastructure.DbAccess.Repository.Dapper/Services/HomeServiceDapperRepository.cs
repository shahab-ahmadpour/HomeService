using App.Domain.Core.DTO.HomeServices;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Dapper.Services
{
    public class HomeServiceDapperRepository : DapperRepositoryBase, IHomeServiceRepository
    {
        public HomeServiceDapperRepository(string connectionString, ILogger logger)
            : base(connectionString, logger)
        {
        }

        public async Task<bool> CreateAsync(CreateHomeServiceDto dto, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Creating new home service: {dto.Name}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        INSERT INTO HomeServices (Name, Description, ImagePath, CategoryId, IsActive)
                        VALUES (@Name, @Description, @ImagePath, @CategoryId, 1)";

                    var parameters = new
                    {
                        dto.Name,
                        dto.Description,
                        dto.ImagePath,
                        dto.CategoryId
                    };

                    var result = await connection.ExecuteAsync(query, parameters);
                    _logger.Information("Successfully created home service: {Name}", dto.Name);
                    return result > 0;
                }
            });
        }

        public async Task<bool> UpdateAsync(int id, UpdateHomeServiceDto dto, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Updating home service with ID: {id}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var existsQuery = "SELECT COUNT(1) FROM HomeServices WHERE Id = @Id";
                    var exists = await connection.ExecuteScalarAsync<int>(existsQuery, new { Id = id });

                    if (exists == 0)
                    {
                        _logger.Warning("Home service with ID: {Id} not found", id);
                        return false;
                    }

                    string imagePath = dto.ImagePath;
                    if (!string.IsNullOrEmpty(imagePath) && !imagePath.StartsWith("/"))
                    {
                        imagePath = "/" + imagePath;
                        _logger.Information("ImagePath updated to: {ImagePath}", imagePath);
                    }
                    else if (string.IsNullOrEmpty(imagePath))
                    {
                        var currentImageQuery = "SELECT ImagePath FROM HomeServices WHERE Id = @Id";
                        imagePath = await connection.ExecuteScalarAsync<string>(currentImageQuery, new { Id = id });
                        _logger.Information("ImagePath is empty, keeping the existing ImagePath: {ImagePath}", imagePath);
                    }

                    var updateQuery = @"
                        UPDATE HomeServices 
                        SET Name = @Name, 
                            Description = @Description, 
                            CategoryId = @CategoryId,
                            ImagePath = @ImagePath,
                            IsActive = @IsActive
                        WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        dto.Name,
                        dto.Description,
                        dto.CategoryId,
                        ImagePath = imagePath,
                        dto.IsActive
                    };

                    var result = await connection.ExecuteAsync(updateQuery, parameters);
                    _logger.Information("Successfully updated home service with ID: {Id}", id);
                    return result > 0;
                }
            });
        }

        public async Task<HomeServiceDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Fetching home service with ID: {id}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT Id, Name, Description, ImagePath, CategoryId, IsActive 
                        FROM HomeServices
                        WHERE Id = @Id";

                    var parameters = new { Id = id };
                    var homeService = await connection.QueryFirstOrDefaultAsync<HomeServiceDto>(query, parameters);

                    if (homeService == null)
                    {
                        _logger.Warning("Home service with ID: {Id} not found", id);
                    }
                    else
                    {
                        homeService.ImagePath = homeService.ImagePath?.Replace("\\", "/");
                        _logger.Information("Fetched home service with ID: {Id}", id);
                    }

                    return homeService;
                }
            });
        }

        public async Task<List<HomeServiceListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync("Fetching all home services", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT hs.Id, hs.Name, hs.IsActive, c.Name as CategoryName
                        FROM HomeServices hs
                        INNER JOIN Categories c ON hs.CategoryId = c.Id";

                    var homeServices = await connection.QueryAsync<HomeServiceListItemDto>(query);
                    var homeServiceList = homeServices.ToList();

                    _logger.Information("Fetched {Count} home services", homeServiceList.Count);
                    return homeServiceList;
                }
            });
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Deleting (deactivating) home service with ID: {id}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var existsQuery = "SELECT COUNT(1) FROM HomeServices WHERE Id = @Id";
                    var exists = await connection.ExecuteScalarAsync<int>(existsQuery, new { Id = id });

                    if (exists == 0)
                    {
                        _logger.Warning("Home service with ID: {Id} not found for deletion", id);
                        return false;
                    }

                    var updateQuery = @"
                        UPDATE HomeServices 
                        SET IsActive = 0
                        WHERE Id = @Id";

                    var parameters = new { Id = id };
                    var result = await connection.ExecuteAsync(updateQuery, parameters);

                    _logger.Information("Successfully deactivated home service with ID: {Id}", id);
                    return result > 0;
                }
            });
        }

        public async Task<List<HomeService>> GetAllWithSubServicesAsync(CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync("Fetching all home services with sub-services", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var homeServices = await connection.QueryAsync<HomeService>("SELECT * FROM HomeServices");
                    var homeServiceList = homeServices.ToList();

                    foreach (var homeService in homeServiceList)
                    {
                        var subServicesQuery = "SELECT * FROM SubHomeServices WHERE HomeServiceId = @HomeServiceId AND IsActive = 1";
                        var subServices = await connection.QueryAsync<SubHomeService>(subServicesQuery, new { HomeServiceId = homeService.Id });
                        homeService.SubHomeServices = subServices.ToList();
                    }

                    return homeServiceList;
                }
            });
        }

        public async Task<List<HomeService>> GetAllHomeServicesAsync(CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync("Fetching all HomeServices with their Categories", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    string query = @"
                        SELECT hs.*, c.* 
                        FROM HomeServices hs
                        INNER JOIN Categories c ON hs.CategoryId = c.Id";

                    var homeServiceDict = new Dictionary<int, HomeService>();

                    var result = await connection.QueryAsync<HomeService, Category, HomeService>(
                        query,
                        (homeService, category) =>
                        {
                            if (!homeServiceDict.TryGetValue(homeService.Id, out var currentHomeService))
                            {
                                currentHomeService = homeService;
                                homeServiceDict.Add(currentHomeService.Id, currentHomeService);
                            }

                            currentHomeService.Category = category;
                            return currentHomeService;
                        },
                        splitOn: "Id"
                    );

                    var homeServiceList = homeServiceDict.Values.ToList();
                    _logger.Information("Fetched {Count} HomeServices with Categories", homeServiceList.Count);
                    return homeServiceList;
                }
            });
        }
    }
}