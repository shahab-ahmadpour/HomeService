using App.Domain.Core.Locations.Interfaces.IAppService;
using App.Domain.Core.Locations.Interfaces.IService;
using App.Domain.Core.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace HomeService.Domain.AppServices.LocationAppServices
{
    public class LocationAppService : ILocationAppService
    {
        private readonly ILocationService _locationService;
        private readonly ILogger _logger;

        public LocationAppService(ILocationService locationService, ILogger logger)
        {
            _locationService = locationService;
            _logger = logger;
        }

        public async Task<List<Province>> GetAllProvincesAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all provinces");
            return await _locationService.GetAllProvincesAsync(cancellationToken);
        }

        public async Task<List<City>> GetAllCitiesAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all cities");
            return await _locationService.GetAllCitiesAsync(cancellationToken);
        }
    }
}
