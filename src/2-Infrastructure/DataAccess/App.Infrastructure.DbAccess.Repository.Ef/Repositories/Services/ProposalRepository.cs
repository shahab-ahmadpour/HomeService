using App.Domain.Core.DTO.Proposals;
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
                    OrderId = p.OrderId,
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
                    OrderDate = p.Order.CreatedAt,
                    SubHomeServiceName = p.Request.SubHomeService.Name,
                    PaymentStatus = p.Order.PaymentStatus.ToString()
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
                    OrderId = p.OrderId,
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
                    OrderDate = p.Order.CreatedAt,
                    SubHomeServiceName = p.Request.SubHomeService.Name,
                    PaymentStatus = p.Order.PaymentStatus.ToString()
                })
                .ToListAsync(cancellationToken);

            _logger.Information("Fetched {Count} proposals for order ID: {OrderId}", proposals.Count, orderId);
            return proposals;
        }

        public async Task<List<ProposalDto>> GetProposalsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching proposals for CustomerId: {CustomerId}", customerId);

            var proposals = await _dbContext.Proposals
                .Include(p => p.Expert.AppUser)
                .Include(p => p.Request)
                .ThenInclude(r => r.Orders)
                .ThenInclude(o => o.Request.SubHomeService)
                .Include(p => p.Order)
                .Where(p => p.Request.Orders.Any(o => o.CustomerId == customerId) && p.IsEnabled)
                .Select(p => new ProposalDto
                {
                    Id = p.Id,
                    ExpertId = p.ExpertId,
                    ExpertName = p.Expert.AppUser.FirstName + " " + p.Expert.AppUser.LastName,
                    OrderId = p.OrderId,
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
                    OrderDate = p.Order.CreatedAt,
                    SubHomeServiceName = p.Request.SubHomeService.Name,
                    PaymentStatus = p.Order.PaymentStatus.ToString()
                })
                .ToListAsync(cancellationToken);

            _logger.Information("Fetched {Count} proposals for CustomerId: {CustomerId}", proposals.Count, customerId);
            return proposals;
        }
    }

}

