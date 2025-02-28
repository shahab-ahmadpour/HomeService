using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Services.Interfaces.IRepository
{
    public interface IProposalRepository
    {
        Task<List<ProposalDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<ProposalDto>> GetProposalsByOrderIdAsync(int orderId, CancellationToken cancellationToken);
        Task<List<ProposalDto>> GetProposalsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
    }

}
