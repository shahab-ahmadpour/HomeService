using App.Domain.Core._ِDTO.Proposals;
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
        Task<bool> CreateAsync(CreateProposalDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, UpdateProposalDto dto, CancellationToken cancellationToken);
        Task<ProposalDto> GetAsync(int id, CancellationToken cancellationToken);
        Task<List<ProposalDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    }
}
