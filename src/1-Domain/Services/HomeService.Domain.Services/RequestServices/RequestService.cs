using App.Domain.Core.DTO.Requests;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Domain.Core.Services.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.Services.RequestServices
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger _logger;

        public RequestService(IRequestRepository requestRepository, ILogger logger)
        {
            _requestRepository = requestRepository;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateRequestDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Creating new request for Customer ID: {CustomerId}", dto.CustomerId);
            var result = await _requestRepository.CreateAsync(dto, cancellationToken);
            _logger.Information("Service: CreateAsync returned: {Result}", result);
            return result;
        }

        public async Task<List<RequestDto>> GetRequestsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Fetching requests for CustomerId: {CustomerId}", customerId);
            var requests = await _requestRepository.GetRequestsByCustomerIdAsync(customerId, cancellationToken);
            _logger.Information("Service: Retrieved {Count} requests for CustomerId: {CustomerId}", requests.Count, customerId);
            return requests;
        }
    }
}
