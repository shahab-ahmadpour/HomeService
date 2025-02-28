using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Domain.Core.Services.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.Services.ProposalServices
{
    public class ProposalService : IProposalService
    {
        private readonly IProposalRepository _proposalRepository;
        private readonly ILogger _logger;

        public ProposalService(IProposalRepository proposalRepository, ILogger logger)
        {
            _proposalRepository = proposalRepository;
            _logger = logger;
            
        }

        public async Task<List<ProposalDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Service: Fetching all proposals.");
            return await _proposalRepository.GetAllAsync(cancellationToken);
        }

        public async Task<List<ProposalDto>> GetProposalsByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Getting proposals for order ID: {OrderId}", orderId);
            return await _proposalRepository.GetProposalsByOrderIdAsync(orderId, cancellationToken);
        }

    }
}
