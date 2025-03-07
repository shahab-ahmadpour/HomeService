using App.Domain.Core.DTO.Users.AppUsers;
using App.Domain.Core.DTO.Users.Experts;
using App.Domain.Core.Users.Interfaces.IAppService;
using App.Domain.Core.Users.Interfaces.IService;
using Serilog;
using System.Threading;

namespace App.Domain.Core.Users.AppServices
{
    public class ExpertAppService : IExpertAppService
    {
        private readonly IExpertService _expertService;
        private readonly IUserAppService _userAppService;
        private readonly ILogger _logger;

        public ExpertAppService(
            IExpertService expertService,
            IUserAppService userAppService,
            ILogger logger)
        {
            _expertService = expertService;
            _userAppService = userAppService;
            _logger = logger;
        }

        public async Task<int?> GetAppUserIdByExpertIdAsync(int expertId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching AppUserId for ExpertId: {ExpertId}", expertId);
            var expert = await _expertService.GetByIdAsync(expertId, cancellationToken);
            if (expert == null)
            {
                _logger.Warning("Expert not found for ExpertId: {ExpertId}", expertId);
                return null;
            }

            return expert.AppUserId;
        }

        public async Task<AppUserDto?> GetExpertUserByExpertIdAsync(int expertId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching expert user for ExpertId: {ExpertId}", expertId);
            var expert = await _expertService.GetByIdAsync(expertId, cancellationToken);
            if (expert == null)
            {
                _logger.Warning("Expert not found for ExpertId: {ExpertId}", expertId);
                return null;
            }

            var user = await _userAppService.GetByIdAsync(expert.AppUserId, cancellationToken);
            if (user == null)
            {
                _logger.Warning("User not found for AppUserId: {AppUserId}", expert.AppUserId);
                return null;
            }

            return user;
        }

        public async Task<List<ExpertDto>> GetAllExpertsAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all experts");
            return await _expertService.GetAllAsync(cancellationToken);
        }

        public async Task<bool> CreateExpertAsync(CreateExpertDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Creating expert with AppUserId: {AppUserId}", dto.AppUserId);
            return await _expertService.CreateAsync(dto, cancellationToken);
        }

        public async Task<bool> UpdateExpertAsync(int id, UpdateExpertDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Updating expert with Id: {Id}", id);
            return await _expertService.UpdateAsync(id, dto, cancellationToken);
        }

        public async Task<bool> DeleteExpertAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Deleting expert with Id: {Id}", id);
            return await _expertService.DeleteAsync(id, cancellationToken);
        }
        public async Task<decimal> GetBalanceAsync(int expertId, CancellationToken cancellationToken)
        {
            return await _expertService.GetBalanceAsync(expertId, cancellationToken);
        }

        public async Task<bool> UpdateBalanceAsync(int expertId, decimal newBalance, CancellationToken cancellationToken)
        {
            return await _expertService.UpdateBalanceAsync(expertId, newBalance, cancellationToken);
        }
    }
}