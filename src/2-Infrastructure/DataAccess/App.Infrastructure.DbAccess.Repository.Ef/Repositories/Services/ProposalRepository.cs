using App.Domain.Core._ِDTO.Proposals;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Services
{
        public class ProposalRepository : IProposalRepository
        {
            private readonly AppDbContext _dbContext;

            public ProposalRepository(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> CreateAsync(CreateProposalDto dto, CancellationToken cancellationToken)
            {
                try
                {
                    var proposal = new Proposal
                    {
                        ExpertId = dto.ExpertId,
                        RequestId = dto.RequestId,
                        SkillId = dto.SkillId,
                        Price = dto.Price,
                        ExecutionDate = dto.ExecutionDate,
                        Description = dto.Description,
                        Status = dto.Status,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _dbContext.Proposals.AddAsync(proposal, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            public async Task<bool> UpdateAsync(int id, UpdateProposalDto dto, CancellationToken cancellationToken)
            {
                var proposal = await _dbContext.Proposals.FindAsync(id);
                if (proposal == null) return false;

                proposal.Status = dto.Status;
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }

            public async Task<ProposalDto> GetAsync(int id, CancellationToken cancellationToken)
            {
                var proposal = await _dbContext.Proposals
                    .Where(p => p.Id == id)
                    .Select(p => new ProposalDto
                    {
                        Id = p.Id,
                        ExpertId = p.ExpertId,
                        RequestId = p.RequestId,
                        Price = p.Price,
                        ExecutionDate = p.ExecutionDate,
                        Description = p.Description,
                        Status = p.Status
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return proposal;
            }

            public async Task<List<ProposalDto>> GetAllAsync(CancellationToken cancellationToken)
            {
                var proposals = await _dbContext.Proposals
                    .Select(p => new ProposalDto
                    {
                        Id = p.Id,
                        ExpertId = p.ExpertId,
                        RequestId = p.RequestId,
                        Price = p.Price,
                        ExecutionDate = p.ExecutionDate,
                        Description = p.Description,
                        Status = p.Status
                    })
                    .ToListAsync(cancellationToken);

                return proposals;
            }

            public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
            {
                var proposal = await _dbContext.Proposals.FindAsync(id);
                if (proposal == null) return false;

                proposal.IsEnabled = false;
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
        }


}
