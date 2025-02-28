using App.Domain.Core.DTO.Requests;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IService;
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

        public RequestAppService(IRequestService requestService, ILogger logger)
        {
            _requestService = requestService;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateRequestDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Creating new request for Customer ID: {CustomerId}", dto.CustomerId);
            var result = await _requestService.CreateAsync(dto, cancellationToken);
            _logger.Information("AppService: CreateAsync returned: {Result}", result);
            return result;
        }

        public async Task<List<RequestDto>> GetRequestsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching requests for CustomerId: {CustomerId}", customerId);
            var requests = await _requestService.GetRequestsByCustomerIdAsync(customerId, cancellationToken);
            _logger.Information("AppService: Retrieved {Count} requests for CustomerId: {CustomerId}", requests.Count, customerId);
            return requests;
        }
    }
}
