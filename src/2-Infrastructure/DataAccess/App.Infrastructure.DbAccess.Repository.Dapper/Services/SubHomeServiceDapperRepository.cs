using App.Domain.Core.DTO.SubHomeServices;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Dapper.Services
{
    public class SubHomeServiceDapperRepository : DapperRepositoryBase, ISubHomeServiceRepository
    {
        private readonly ILogger _logger;

        public SubHomeServiceDapperRepository(string connectionString, ILogger logger)
            : base(connectionString, logger)
        {
            _logger = logger;
        }

        public async Task<bool> CreateAsync(SubHomeService subHomeService, CancellationToken cancellationToken)
        {
            _logger.Information("Creating new SubHomeService with name: {Name}", subHomeService.Name);

            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        INSERT INTO SubHomeServices (Name, Description, Views, BasePrice, ImagePath, HomeServiceId, IsActive)
                        VALUES (@Name, @Description, @Views, @BasePrice, @ImagePath, @HomeServiceId, @IsActive)";

                    var result = await connection.ExecuteAsync(query, subHomeService);
                    _logger.Information("SubHomeService with name {Name} created successfully.", subHomeService.Name);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating SubHomeService with name: {Name}", subHomeService.Name);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateSubHomeServiceDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Updating SubHomeService with Id: {Id}", id);

            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var checkQuery = "SELECT COUNT(1) FROM SubHomeServices WHERE Id = @Id";
                    var exists = await connection.ExecuteScalarAsync<int>(checkQuery, new { Id = id });

                    if (exists == 0)
                    {
                        _logger.Warning("SubHomeService with Id: {Id} not found.", id);
                        return false;
                    }

                    string imagePath = dto.ImagePath;
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        var imageQuery = "SELECT ImagePath FROM SubHomeServices WHERE Id = @Id";
                        imagePath = await connection.ExecuteScalarAsync<string>(imageQuery, new { Id = id });
                        _logger.Warning("ImagePath is empty, keeping the existing ImagePath: {ImagePath}", imagePath);
                    }

                    var updateQuery = @"
                        UPDATE SubHomeServices
                        SET Name = @Name,
                            Description = @Description,
                            Views = @Views,
                            BasePrice = @BasePrice,
                            ImagePath = @ImagePath,
                            IsActive = @IsActive
                        WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        dto.Name,
                        dto.Description,
                        dto.Views,
                        dto.BasePrice,
                        ImagePath = imagePath,
                        dto.IsActive
                    };

                    var result = await connection.ExecuteAsync(updateQuery, parameters);
                    _logger.Information("SubHomeService with Id: {Id} updated successfully.", id);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating SubHomeService with Id: {Id}.", id);
                return false;
            }
        }

        public async Task<SubHomeServiceDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching SubHomeService with Id: {Id}", id);

            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT s.Id, s.Name, s.Description, s.Views, s.BasePrice, s.ImagePath, s.IsActive, 
                               s.HomeServiceId, h.Name as HomeServiceName
                        FROM SubHomeServices s
                        INNER JOIN HomeServices h ON s.HomeServiceId = h.Id
                        WHERE s.Id = @Id";

                    var subHomeService = await connection.QueryFirstOrDefaultAsync<SubHomeServiceDto>(
                        query, new { Id = id });

                    if (subHomeService == null)
                    {
                        _logger.Warning("SubHomeService with Id: {Id} not found.", id);
                    }

                    return subHomeService;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching SubHomeService with Id: {Id}", id);
                throw;
            }
        }

        public async Task<List<SubHomeServiceListItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all SubHomeServices.");

            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT s.Id, s.Name, s.Views, s.Description, s.BasePrice, s.ImagePath, s.IsActive,
                               h.Name as HomeServiceName
                        FROM SubHomeServices s
                        INNER JOIN HomeServices h ON s.HomeServiceId = h.Id";

                    var subHomeServices = await connection.QueryAsync<SubHomeServiceListItemDto>(query);
                    return subHomeServices.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching all SubHomeServices");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Disabling SubHomeService with Id: {Id}", id);

            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var checkQuery = "SELECT COUNT(1) FROM SubHomeServices WHERE Id = @Id";
                    var exists = await connection.ExecuteScalarAsync<int>(checkQuery, new { Id = id });

                    if (exists == 0)
                    {
                        _logger.Warning("SubHomeService with Id: {Id} not found.", id);
                        return false;
                    }

                    var updateQuery = "UPDATE SubHomeServices SET IsActive = 0 WHERE Id = @Id";
                    var result = await connection.ExecuteAsync(updateQuery, new { Id = id });

                    _logger.Information("SubHomeService with Id: {Id} successfully disabled.", id);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error disabling SubHomeService with Id: {Id}.", id);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Checking if SubHomeService with Id: {Id} exists.", id);

            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = "SELECT COUNT(1) FROM SubHomeServices WHERE Id = @Id";
                    var exists = await connection.ExecuteScalarAsync<int>(query, new { Id = id });
                    return exists > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error checking if SubHomeService with Id: {Id} exists.", id);
                throw;
            }
        }

        public async Task<List<SubHomeService>> GetAllServicesAsync(CancellationToken cancellationToken = default)
        {
            _logger.Information("Fetching all SubHomeServices asynchronously.");
            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = "SELECT * FROM SubHomeServices";
                    var services = await connection.QueryAsync<SubHomeService>(query);
                    var serviceList = services.ToList();

                    _logger.Information("Fetched {Count} SubHomeServices.", serviceList?.Count ?? 0);
                    return serviceList;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch all SubHomeServices asynchronously.");
                throw;
            }
        }

        public async Task<SubHomeServiceListItemDto> GetSubHomeServiceByIdAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching SubHomeService by Id: {Id}", id);
            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT s.Id, s.Name, s.Views, s.Description, s.BasePrice, s.ImagePath, s.IsActive,
                               s.HomeServiceId, h.Name as HomeServiceName
                        FROM SubHomeServices s
                        INNER JOIN HomeServices h ON s.HomeServiceId = h.Id
                        WHERE s.Id = @Id";

                    var subHomeService = await connection.QueryFirstOrDefaultAsync<SubHomeServiceListItemDto>(
                        query, new { Id = id });

                    if (subHomeService == null)
                    {
                        _logger.Warning("SubHomeService not found for Id: {Id}", id);
                    }
                    else
                    {
                        _logger.Information("Found SubHomeService with Id: {Id}", id);
                    }

                    return subHomeService;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch SubHomeService for Id: {Id}", id);
                throw;
            }
        }

        public async Task<List<SubHomeServiceListItemDto>> GetSubHomeServicesByHomeServiceIdAsync(int homeServiceId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching sub-home services for HomeServiceId: {HomeServiceId}", homeServiceId);
            try
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT s.Id, s.Name, s.Description, s.BasePrice, s.Views, s.ImagePath
                        FROM SubHomeServices s
                        WHERE s.HomeServiceId = @HomeServiceId";

                    var parameters = new { HomeServiceId = homeServiceId };
                    var subHomeServices = await connection.QueryAsync<SubHomeServiceListItemDto>(query, parameters);
                    var subHomeServiceList = subHomeServices.ToList();

                    if (subHomeServiceList == null || !subHomeServiceList.Any())
                    {
                        _logger.Warning("No sub-home services found for HomeServiceId: {HomeServiceId}", homeServiceId);
                        return new List<SubHomeServiceListItemDto>();
                    }

                    _logger.Information("Found {Count} sub-home services for HomeServiceId: {HomeServiceId}",
                                       subHomeServiceList.Count, homeServiceId);
                    return subHomeServiceList;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch sub-home services for HomeServiceId: {HomeServiceId}", homeServiceId);
                throw;
            }
        }
    }
}