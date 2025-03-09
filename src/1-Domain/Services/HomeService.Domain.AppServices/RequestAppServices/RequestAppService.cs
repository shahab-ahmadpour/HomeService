using App.Domain.Core.DTO.Requests;
using App.Domain.Core.Enums;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Domain.Core.Services.Interfaces.IService;
using App.Domain.Core.Skills.Interfaces;
using App.Domain.Core.Users.Interfaces.IRepository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.RequestAppServices
{
    public class RequestAppService : IRequestAppService
    {
        private readonly IRequestService _requestService;
        private readonly IExpertRepository _expertRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ILogger _logger;

        public RequestAppService(
            IRequestService requestService,
            IExpertRepository expertRepository,
            ISkillRepository skillRepository,
            ILogger logger)
        {
            _requestService = requestService;
            _expertRepository = expertRepository;
            _skillRepository = skillRepository;
            _logger = logger;
        }

        public async Task<bool> CreateRequestAsync(CreateRequestDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Creating request for CustomerId: {CustomerId}, SubHomeServiceId: {SubHomeServiceId}", dto.CustomerId, dto.SubHomeServiceId);
            return await _requestService.CreateAsync(dto, cancellationToken);
        }

        public async Task<List<RequestDto>> GetRequestsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("Getting requests for CustomerId: {CustomerId}", customerId);
            return await _requestService.GetRequestsByCustomerIdAsync(customerId, cancellationToken);
        }

        public async Task<bool> UpdateAsync(int id, UpdateRequestDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Updating request with Id: {Id}", id);
            return await _requestService.UpdateAsync(id, dto, cancellationToken);
        }

        public async Task<RequestDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Getting request with Id: {Id}", id);
            return await _requestService.GetAsync(id, cancellationToken);
        }

        public async Task<List<RequestDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Getting all requests");
            return await _requestService.GetAllAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Deleting request with Id: {Id}", id);
            return await _requestService.DeleteAsync(id, cancellationToken);
        }

        public async Task<List<RequestDto>> GetAvailableRequestsForExpertAsync(int expertId, CancellationToken cancellationToken)
        {
            _logger.Information("Getting available requests for ExpertId: {ExpertId}", expertId);

            try
            {
                // Get all pending requests
                var allRequests = await _requestService.GetAllAsync(cancellationToken);
                var pendingRequests = allRequests.Where(r => r.Status == RequestStatus.Pending && r.IsEnabled).ToList();

                // Get expert's skills
                var expert = await _expertRepository.GetByIdAsync(expertId, cancellationToken);
                if (expert == null)
                {
                    _logger.Warning("Expert not found for ExpertId: {ExpertId}", expertId);
                    return new List<RequestDto>();
                }

                // Get expert's skill IDs
                var expertSkills = await _skillRepository.GetSkillsByExpertIdAsync(expertId, cancellationToken);
                if (expertSkills == null || !expertSkills.Any())
                {
                    _logger.Information("Expert with ID: {ExpertId} has no skills, returning all pending requests", expertId);
                    return pendingRequests;
                }

                var expertSubServiceIds = expertSkills.Select(s => s.SubHomeServiceId).Distinct().ToList();

                // Filter requests by expert's skills
                var availableRequests = pendingRequests.Where(r => expertSubServiceIds.Contains(r.SubHomeServiceId)).ToList();

                _logger.Information("Found {Count} available requests for ExpertId: {ExpertId}", availableRequests.Count, expertId);
                return availableRequests;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting available requests for ExpertId: {ExpertId}", expertId);
                return new List<RequestDto>();
            }
        }
    }
}