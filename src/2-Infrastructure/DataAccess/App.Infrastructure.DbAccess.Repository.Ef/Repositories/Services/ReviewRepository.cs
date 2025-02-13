using App.Domain.Core._ِDTO.Reviews;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
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

        public ReviewRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(CreateReviewDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var review = new Review
                {
                    CustomerId = dto.CustomerId,
                    ExpertId = dto.ExpertId,
                    OrderId = dto.OrderId,
                    Rating = dto.Rating,
                    Comment = dto.Comment,
                };

                await _dbContext.Reviews.AddAsync(review, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateAsync(int id, UpdateReviewDto dto, CancellationToken cancellationToken)
        {
            var review = await _dbContext.Reviews.FindAsync(id);
            if (review == null) return false;

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<ReviewDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var review = await _dbContext.Reviews
                .Where(r => r.Id == id)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    ExpertId = r.ExpertId,
                    OrderId = r.OrderId,
                    Rating = r.Rating,
                    Comment = r.Comment
                })
                .FirstOrDefaultAsync(cancellationToken);

            return review;
        }
        public async Task<List<ReviewDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var reviews = await _dbContext.Reviews
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    ExpertId = r.ExpertId,
                    OrderId = r.OrderId,
                    Rating = r.Rating,
                    Comment = r.Comment
                })
                .ToListAsync(cancellationToken);

            return reviews;
        }
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var review = await _dbContext.Reviews.FindAsync(id);
            if (review == null) return false;

            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
