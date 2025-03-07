using App.Domain.Core.DTO.Requests;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IService;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.RequestAppServices
{
    public class RequestAppService : IRequestAppService
    {
        private readonly IRequestService _requestService;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public RequestAppService(
            IRequestService requestService,
            ILogger logger,
            IMemoryCache memoryCache)
        {
            _requestService = requestService;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<bool> CreateRequestAsync(CreateRequestDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Creating request for CustomerId: {CustomerId} in AppService layer.", dto.CustomerId);
            try
            {
                var result = await _requestService.CreateAsync(dto, cancellationToken);
                _logger.Information("Request creation result for CustomerId: {CustomerId} - Success: {Result}", dto.CustomerId, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create request for CustomerId: {CustomerId} in AppService layer.", dto.CustomerId);
                throw;
            }
        }

        public async Task<List<RequestDto>> GetRequestsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching requests for CustomerId: {CustomerId}", customerId);
            string cacheKey = $"Requests_Customer_{customerId}";

            if (!_memoryCache.TryGetValue(cacheKey, out List<RequestDto> cachedRequests))
            {
                _logger.Information("Requests not found in cache for CustomerId: {CustomerId}, fetching from database", customerId);
                cachedRequests = await _requestService.GetRequestsByCustomerIdAsync(customerId, cancellationToken);
                if (cachedRequests != null && cachedRequests.Any())
                {
                    _logger.Information("Caching {RequestCount} requests for CustomerId: {CustomerId}", cachedRequests.Count, customerId);
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    };
                    _memoryCache.Set(cacheKey, cachedRequests, cacheOptions);
                }
            }
            else
            {
                _logger.Information("Requests retrieved from cache for CustomerId: {CustomerId}, Count: {RequestCount}", customerId, cachedRequests.Count);
            }
            return cachedRequests;
        }

        public async Task<bool> UpdateAsync(int id, UpdateRequestDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Updating request with ID: {RequestId}", id);
            var result = await _requestService.UpdateAsync(id, dto, cancellationToken);
            _logger.Information("AppService: UpdateAsync returned: {Result}", result);
            return result;
        }

        public async Task<RequestDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching request with ID: {RequestId}", id);
            var request = await _requestService.GetAsync(id, cancellationToken);
            _logger.Information("AppService: Fetched request with ID: {RequestId}", id);
            return request;
        }

        public async Task<List<RequestDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all requests.");
            var requests = await _requestService.GetAllAsync(cancellationToken);
            _logger.Information("AppService: Retrieved {Count} requests.", requests.Count);
            return requests;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Deleting (deactivating) request with ID: {RequestId}", id);
            var result = await _requestService.DeleteAsync(id, cancellationToken);
            _logger.Information("AppService: DeleteAsync returned: {Result}", result);
            return result;
        }
    }
}
