using App.Domain.Core.DTO.City;
using App.Domain.Core.Locations;
using App.Domain.Core.Locations.Interfaces.IRepository;
using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Dapper.Locations
{
    public class LocationDapperRepository : DapperRepositoryBase, ILocationRepository
    {
        public LocationDapperRepository(string connectionString, ILogger logger)
            : base(connectionString, logger)
        {
        }

        public async Task<List<Province>> GetAllProvincesAsync(CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync("Fetching all provinces", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = "SELECT Id, Name FROM Provinces";
                    var provinces = await connection.QueryAsync<Province>(query);
                    return provinces.ToList();
                }
            });
        }

        public async Task<List<City>> GetAllCitiesAsync(CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync("Fetching all cities", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = "SELECT Id, Name, ProvinceId FROM Cities";
                    var cities = await connection.QueryAsync<City>(query);
                    return cities.ToList();
                }
            });
        }

        public async Task<List<CityDto>> GetCitiesByProvinceIdAsync(int provinceId, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Fetching cities for ProvinceId: {provinceId}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = "SELECT Name FROM Cities WHERE ProvinceId = @ProvinceId";
                    var parameters = new { ProvinceId = provinceId };
                    var cities = await connection.QueryAsync<CityDto>(query, parameters);

                    var cityList = cities.ToList();
                    _logger.Information("Found {Count} cities for ProvinceId: {ProvinceId}", cityList.Count, provinceId);
                    return cityList;
                }
            });
        }

        public async Task<List<CityDto>> GetCitiesByProvinceNameAsync(string provinceName, CancellationToken cancellationToken)
        {
            return await ExecuteWithLoggingAsync($"Fetching cities for ProvinceName: {provinceName}", async () =>
            {
                using (var connection = await CreateOpenConnectionAsync())
                {
                    var query = @"
                        SELECT c.Name 
                        FROM Cities c
                        INNER JOIN Provinces p ON c.ProvinceId = p.Id
                        WHERE p.Name = @ProvinceName";

                    var parameters = new { ProvinceName = provinceName };
                    var cities = await connection.QueryAsync<CityDto>(query, parameters);

                    var cityList = cities.ToList();
                    _logger.Information("Found {Count} cities for ProvinceName: {ProvinceName}", cityList.Count, provinceName);
                    return cityList;
                }
            });
        }
    }
}