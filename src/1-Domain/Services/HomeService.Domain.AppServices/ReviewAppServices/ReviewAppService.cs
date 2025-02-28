using App.Domain.Core.DTO.Reviews;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.ReviewAppServices
{
    public class ReviewAppService : IReviewAppService
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger _logger;

        public ReviewAppService(IReviewService reviewService, ILogger logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        public async Task<List<ReviewDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all reviews.");
            return await _reviewService.GetAllAsync(cancellationToken);
        }

        public async Task<List<ReviewDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching reviews for order ID: {OrderId}", orderId);
            return await _reviewService.GetByOrderIdAsync(orderId, cancellationToken);
        }


        public async Task<bool> ApproveAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Approving review with ID: {Id}", id);
            var result = await _reviewService.ApproveAsync(id, cancellationToken);

            if (result)
                _logger.Information("AppService: Review with ID: {Id} approved successfully.", id);
            else
                _logger.Warning("AppService: Failed to approve review with ID: {Id}.", id);

            return result;
        }

        public async Task<bool> RejectAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Rejecting review with ID: {Id}", id);
            var result = await _reviewService.RejectAsync(id, cancellationToken);

            if (result)
                _logger.Information("AppService: Review with ID: {Id} rejected successfully.", id);
            else
                _logger.Warning("AppService: Failed to reject review with ID: {Id}.", id);

            return result;
        }

    }

}
