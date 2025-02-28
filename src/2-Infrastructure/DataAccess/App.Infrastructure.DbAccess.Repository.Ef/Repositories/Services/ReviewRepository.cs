using App.Domain.Core.DTO.Reviews;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Services
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public ReviewRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<ReviewDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all reviews.");
            var reviews = await _dbContext.Reviews
                .Include(r => r.Customer.AppUser)
                .Include(r => r.Expert.AppUser)
                .Include(r => r.Order)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer.AppUser.FirstName + " " + r.Customer.AppUser.LastName,
                    ExpertId = r.ExpertId,
                    ExpertName = r.Expert.AppUser.FirstName + " " + r.Expert.AppUser.LastName,
                    OrderId = r.OrderId,
                    OrderDescription = $"سفارش شماره {r.OrderId}",
                    Rating = r.Rating,
                    Comment = r.Comment,
                    IsApproved = r.IsApproved,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return reviews;
        }

        public async Task<List<ReviewDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching reviews for order ID: {OrderId}", orderId);
            var reviews = await _dbContext.Reviews
                .Include(r => r.Customer.AppUser)
                .Include(r => r.Expert.AppUser)
                .Include(r => r.Order)
                .Where(r => r.OrderId == orderId)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    CustomerName = r.Customer.AppUser.FirstName + " " + r.Customer.AppUser.LastName,
                    ExpertId = r.ExpertId,
                    ExpertName = r.Expert.AppUser.FirstName + " " + r.Expert.AppUser.LastName,
                    OrderId = r.OrderId,
                    OrderDescription = $"سفارش شماره {r.OrderId}",
                    Rating = r.Rating,
                    Comment = r.Comment,
                    IsApproved = r.IsApproved,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return reviews;
        }


        public async Task<bool> ApproveAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Approving review with ID: {Id}", id);
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (review == null)
            {
                _logger.Warning("Review with ID: {Id} not found", id);
                return false;
            }
            review.IsApproved = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> RejectAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Rejecting review with ID: {Id}", id);
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (review == null)
            {
                _logger.Warning("Review with ID: {Id} not found", id);
                return false;
            }
            review.IsApproved = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        public List<Review> GetAllReviews()
        {
            return _dbContext.Reviews.ToList();
        }
    }
}
