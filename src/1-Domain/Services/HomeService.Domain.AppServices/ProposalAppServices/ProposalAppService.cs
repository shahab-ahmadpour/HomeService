using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Domain.Core.Services.Interfaces.IService;
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
        private readonly ILogger _logger;

        public ProposalAppService(IProposalService proposalService, ILogger logger)
        {
            _proposalService = proposalService;
            _logger = logger;

        }

        public async Task<List<ProposalDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all proposals.");
            return await _proposalService.GetAllAsync(cancellationToken);
        }

        public async Task<List<ProposalDto>> GetProposalsByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Getting proposals for order ID: {OrderId}", orderId);
            return await _proposalService.GetProposalsByOrderIdAsync(orderId, cancellationToken);
        }


    }
}
