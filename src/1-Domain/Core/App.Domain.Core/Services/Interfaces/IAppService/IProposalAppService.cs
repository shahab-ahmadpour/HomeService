using App.Domain.Core.DTO.Proposals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Services.Interfaces.IAppService
{
    public interface IProposalAppService
    {
        Task<List<ProposalDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<ProposalDto>> GetProposalsByOrderIdAsync(int orderId, CancellationToken cancellationToken);
    }

}
