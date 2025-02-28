using App.Domain.Core.Locations.Interfaces.IRepository;
using App.Domain.Core.Locations;
using App.Infrastructure.Db.SqlServer.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Locations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public LocationRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Province>> GetAllProvincesAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all provinces");
            try
            {
                return await _dbContext.Provinces
                    .OrderBy(p => p.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch provinces");
                throw;
            }
        }

        public async Task<List<City>> GetAllCitiesAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all cities");
            try
            {
                return await _dbContext.Cities
                    .OrderBy(c => c.Name)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch cities");
                throw;
            }
        }
    }
}
