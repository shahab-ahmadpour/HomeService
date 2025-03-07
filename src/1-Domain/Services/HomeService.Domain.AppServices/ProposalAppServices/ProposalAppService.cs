using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Domain.Core.Services.Interfaces.IService;
using App.Domain.Core.Users.Interfaces.IAppService;
using App.Domain.Core.Users.Interfaces.IService;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.ProposalAppServices
{
    public class ProposalAppService : IProposalAppService
    {
        private readonly IProposalService _proposalService;
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public ProposalAppService(
            IProposalService proposalService,
            ICustomerService customerService,
            ILogger logger,
            IMemoryCache memoryCache)
        {
            _proposalService = proposalService;
            _customerService = customerService;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<List<ProposalDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all proposals.");
            return await _proposalService.GetAllAsync(cancellationToken);
        }

        public async Task<List<ProposalDto>> GetProposalsByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Getting proposals for order ID: {OrderId}", orderId);
            string cacheKey = $"Proposals_Order_{orderId}";

            if (!_memoryCache.TryGetValue(cacheKey, out List<ProposalDto> cachedProposals))
            {
                _logger.Information("Proposals not found in cache for OrderId: {OrderId}, fetching from database", orderId);
                cachedProposals = await _proposalService.GetProposalsByOrderIdAsync(orderId, cancellationToken);
                if (cachedProposals != null && cachedProposals.Any())
                {
                    _logger.Information("Caching {ProposalCount} proposals for OrderId: {OrderId}", cachedProposals.Count, orderId);
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    };
                    _memoryCache.Set(cacheKey, cachedProposals, cacheOptions);
                }
            }
            else
            {
                _logger.Information("Proposals retrieved from cache for OrderId: {OrderId}, Count: {ProposalCount}", orderId, cachedProposals.Count);
            }
            return cachedProposals;
        }
        public async Task<ProposalDto> GetProposalByIdAsync(int proposalId, CancellationToken cancellationToken)
        {
            _logger.Information("ProposalAppService: Fetching proposal by ID: {Id}", proposalId);
            return await _customerService.GetProposalByIdAsync(proposalId, cancellationToken);
        }

        public async Task UpdateProposalAsync(Proposal proposal, CancellationToken cancellationToken)
        {
            _logger.Information("ProposalAppService: Updating proposal with ID: {Id}", proposal.Id);
            await _customerService.UpdateProposalAsync(proposal, cancellationToken);
            _logger.Information("ProposalAppService: Successfully updated proposal with ID: {Id}", proposal.Id);
        }

    }
}
