using App.Domain.Core.DTO.Users.Experts;
using App.Domain.Core.Users.Interfaces.IRepository;
using App.Domain.Core.Users.Interfaces.IService;
using Serilog;
using System.Threading;

namespace App.Domain.Core.Users.Services
{
    public class ExpertService : IExpertService
    {
        private readonly IExpertRepository _expertRepository;
        private readonly ILogger _logger;

        public ExpertService(IExpertRepository expertRepository, ILogger logger)
        {
            _expertRepository = expertRepository;
            _logger = logger;
        }

        public async Task<ExpertDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Fetching Expert with ID: {Id}", id);
            return await _expertRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<ExpertDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Service: Fetching all experts");
            return await _expertRepository.GetAllAsync(cancellationToken);
        }

        public async Task<bool> CreateAsync(CreateExpertDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Creating Expert with AppUserId: {AppUserId}", dto.AppUserId);
            return await _expertRepository.CreateAsync(dto, cancellationToken);
        }

        public async Task<bool> UpdateAsync(int id, UpdateExpertDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Updating Expert with ID: {Id}", id);
            return await _expertRepository.UpdateAsync(id, dto, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Deleting Expert with ID: {Id}", id);
            return await _expertRepository.DeleteAsync(id, cancellationToken);
        }
        public async Task<decimal> GetBalanceAsync(int expertId, CancellationToken cancellationToken)
        {
            return await _expertRepository.GetBalanceAsync(expertId, cancellationToken);
        }

        public async Task<bool> UpdateBalanceAsync(int expertId, decimal newBalance, CancellationToken cancellationToken)
        {
            return await _expertRepository.UpdateBalanceAsync(expertId, newBalance, cancellationToken);
        }
    }
}