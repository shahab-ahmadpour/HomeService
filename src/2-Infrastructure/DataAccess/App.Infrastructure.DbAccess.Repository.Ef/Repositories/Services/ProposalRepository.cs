using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.Enums;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
        private readonly ILogger _logger;

        public ProposalRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<ProposalDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all proposals from database.");
            var proposals = await _dbContext.Proposals
                .Include(p => p.Expert.AppUser)
                .Include(p => p.Request)
                .ThenInclude(r => r.SubHomeService)
                .Include(p => p.Order)
                .Select(p => new ProposalDto
                {
                    Id = p.Id,
                    ExpertId = p.ExpertId,
                    ExpertName = p.Expert.AppUser.FirstName + " " + p.Expert.AppUser.LastName,
                    OrderId = p.OrderId.GetValueOrDefault(),
                    RequestId = p.RequestId,
                    RequestDescription = p.Request.Description,
                    SkillId = p.SkillId,
                    Price = p.Price,
                    ExecutionDate = p.ExecutionDate,
                    Description = p.Description,
                    Status = p.Status,
                    ResponseTime = p.ResponseTime,
                    CreatedAt = p.CreatedAt,
                    IsEnabled = p.IsEnabled,
                    OrderDate = p.Order != null ? p.Order.CreatedAt : DateTime.MinValue,
                    SubHomeServiceName = p.Request.SubHomeService.Name,
                    PaymentStatus = p.Order != null ? p.Order.PaymentStatus.ToString() : PaymentStatus.Pending.ToString() 
                })
                .ToListAsync(cancellationToken);

            _logger.Information("Fetched {Count} proposals from database.", proposals.Count);
            return proposals;
        }

        public async Task<List<ProposalDto>> GetProposalsByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching proposals for order ID: {OrderId}", orderId);

            var proposals = await _dbContext.Proposals
                .Include(p => p.Expert.AppUser)
                .Include(p => p.Request)
                .ThenInclude(r => r.SubHomeService)
                .Include(p => p.Order)
                .Where(p => p.OrderId == orderId && p.IsEnabled)
                .Select(p => new ProposalDto
                {
                    Id = p.Id,
                    ExpertId = p.ExpertId,
                    ExpertName = p.Expert.AppUser.FirstName + " " + p.Expert.AppUser.LastName,
                    OrderId = p.OrderId.GetValueOrDefault(),
                    RequestId = p.RequestId,
                    RequestDescription = p.Request.Description,
                    SkillId = p.SkillId,
                    Price = p.Price,
                    ExecutionDate = p.ExecutionDate,
                    Description = p.Description,
                    Status = p.Status,
                    ResponseTime = p.ResponseTime,
                    CreatedAt = p.CreatedAt,
                    IsEnabled = p.IsEnabled,
                    OrderDate = p.Order != null ? p.Order.CreatedAt : DateTime.MinValue,
                    SubHomeServiceName = p.Request.SubHomeService.Name,
                    PaymentStatus = p.Order != null ? p.Order.PaymentStatus.ToString() : PaymentStatus.Pending.ToString()
                })
                .ToListAsync(cancellationToken);

            _logger.Information("Fetched {Count} proposals for order ID: {OrderId}", proposals.Count, orderId);
            return proposals;
        }

        public async Task<List<ProposalDto>> GetProposalsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("ProposalRepository: Fetching proposals for CustomerId: {CustomerId}", customerId);

            var proposals = await _dbContext.Proposals
                .Include(p => p.Expert)
                .ThenInclude(e => e.AppUser)
                .Include(p => p.Request)
                .ThenInclude(r => r.SubHomeService)
                .Include(p => p.Order)
                .Where(p => p.Request.CustomerId == customerId && p.IsEnabled)
                .ToListAsync(cancellationToken);

            foreach (var p in proposals)
            {
                _logger.Information("Proposal ID: {Id}, Expert: {ExpertId}, AppUser: {AppUser}, SubHomeService: {SubHomeService}, OrderId: {OrderId}",
                    p.Id,
                    p.Expert?.Id,
                    p.Expert?.AppUser != null ? $"{p.Expert.AppUser.FirstName} {p.Expert.AppUser.LastName}" : "null",
                    p.Request?.SubHomeService != null ? p.Request.SubHomeService.Name : "null",
                    p.OrderId ?? 0);
            }

            var proposalDtos = proposals.Select(p => new ProposalDto
            {
                Id = p.Id,
                ExpertId = p.ExpertId,
                ExpertName = p.Expert?.AppUser != null ? p.Expert.AppUser.FirstName + " " + (p.Expert.AppUser.LastName ?? "") : "نامشخص",
                OrderId = p.OrderId,
                RequestId = p.RequestId,
                RequestDescription = p.Request?.Description ?? "بدون توضیح",
                SkillId = p.SkillId,
                Price = p.Price,
                ExecutionDate = p.ExecutionDate,
                Description = p.Description,
                Status = p.Status,
                ResponseTime = p.ResponseTime,
                CreatedAt = p.CreatedAt,
                IsEnabled = p.IsEnabled,
                OrderDate = p.Order != null ? p.Order.CreatedAt : DateTime.MinValue,
                SubHomeServiceName = p.Request?.SubHomeService != null ? p.Request.SubHomeService.Name : "نامشخص",
                PaymentStatus = p.Order != null ? p.Order.PaymentStatus.ToString() : PaymentStatus.Pending.ToString()
            }).ToList();

            foreach (var dto in proposalDtos)
            {
                _logger.Information("ProposalDto ID: {Id}, ExpertName: {ExpertName}, SubHomeServiceName: {SubHomeServiceName}, OrderDate: {OrderDate}",
                    dto.Id,
                    dto.ExpertName,
                    dto.SubHomeServiceName,
                    dto.OrderDate == DateTime.MinValue ? "MinValue" : dto.OrderDate.ToString());
            }

            if (!proposalDtos.Any())
            {
                _logger.Warning("ProposalRepository: No proposals found for CustomerId: {CustomerId}", customerId);
            }
            else
            {
                _logger.Information("ProposalRepository: Fetched {Count} proposals for CustomerId: {CustomerId}", proposalDtos.Count, customerId);
            }

            return proposalDtos;
        }

        public async Task<Proposal> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("ProposalRepository: Fetching proposal with ID: {Id}", id);
            var proposal = await _dbContext.Proposals
                .Include(p => p.Request)
                .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (proposal == null)
            {
                _logger.Warning("ProposalRepository: Proposal not found for ID: {Id}", id);
            }
            else
            {
                _logger.Information("ProposalRepository: Successfully fetched proposal with ID: {Id}", id);
            }

            return proposal;
        }
        public async Task UpdateAsync(Proposal proposal, CancellationToken cancellationToken)
        {
            _logger.Information("ProposalRepository: Updating proposal with ID: {Id}", proposal.Id);
            var existingProposal = await _dbContext.Proposals
                .FirstOrDefaultAsync(p => p.Id == proposal.Id, cancellationToken);

            if (existingProposal == null)
            {
                _logger.Warning("ProposalRepository: Proposal not found for ID: {Id}", proposal.Id);
                return;
            }

            existingProposal.OrderId = proposal.OrderId;
            existingProposal.Status = proposal.Status;
            existingProposal.IsEnabled = proposal.IsEnabled;

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Information("ProposalRepository: Successfully updated proposal with ID: {Id}", proposal.Id);
        }
    }

}

