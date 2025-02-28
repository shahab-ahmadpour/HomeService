using App.Domain.Core.DTO.Reviews;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Domain.Core.Services.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.Services.ReviewServices
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger _logger;

        public ReviewService(IReviewRepository reviewRepository, ILogger logger)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        public async Task<List<ReviewDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all reviews.");
            return await _reviewRepository.GetAllAsync(cancellationToken);
        }

        public async Task<List<ReviewDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching reviews for order ID: {OrderId}", orderId);
            return await _reviewRepository.GetByOrderIdAsync(orderId, cancellationToken);
        }

        public async Task<bool> ApproveAsync(int id, CancellationToken cancellationToken)
        {
            return await _reviewRepository.ApproveAsync(id, cancellationToken);
        }

        public async Task<bool> RejectAsync(int id, CancellationToken cancellationToken)
        {
            return await _reviewRepository.RejectAsync(id, cancellationToken);
        }
        public List<Review> GetAllReviews()
        {
            return _reviewRepository.GetAllReviews();
        }
    }

}
