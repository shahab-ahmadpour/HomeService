using App.Domain.Core.DTO.Users.Experts;
using App.Domain.Core.Users.Entities;
using App.Domain.Core.Users.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Users
{
    public class ExpertRepository : IExpertRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public ExpertRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ExpertDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching expert with ID: {Id}", id);
            try
            {
                var expert = await _dbContext.Experts
                    .Where(e => e.Id == id)
                    .Select(e => new ExpertDto
                    {
                        Id = e.Id,
                        AppUserId = e.AppUserId,
                        PhoneNumber = e.PhoneNumber,
                        Address = e.Address,
                        City = e.City,
                        State = e.State
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (expert == null)
                {
                    _logger.Warning("Expert with ID: {Id} not found", id);
                }
                else
                {
                    _logger.Information("Expert with ID: {Id} fetched successfully", id);
                }

                return expert;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch expert with ID: {Id}", id);
                throw;
            }
        }

        public async Task<List<ExpertDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all experts.");
            try
            {
                var experts = await _dbContext.Experts
                    .Select(e => new ExpertDto
                    {
                        Id = e.Id,
                        AppUserId = e.AppUserId,
                        PhoneNumber = e.PhoneNumber,
                        Address = e.Address,
                        City = e.City,
                        State = e.State
                    })
                    .ToListAsync(cancellationToken);

                _logger.Information("Fetched {Count} experts.", experts.Count);
                return experts;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch all experts.");
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateExpertDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Creating a new expert for AppUser ID: {AppUserId}", dto.AppUserId);
            try
            {
                var expert = new Expert
                {
                    AppUserId = dto.AppUserId,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    City = dto.City,
                    State = dto.State
                };

                await _dbContext.Experts.AddAsync(expert, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.Information("Expert created successfully with ID: {Id}", expert.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create expert for AppUser ID: {AppUserId}", dto.AppUserId);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateExpertDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Updating expert with ID: {Id}", id);
            try
            {
                var expert = await _dbContext.Experts.FindAsync(id);
                if (expert == null)
                {
                    _logger.Warning("Expert with ID: {Id} not found for update", id);
                    return false;
                }

                if (!string.IsNullOrEmpty(dto.PhoneNumber)) expert.PhoneNumber = dto.PhoneNumber;
                if (!string.IsNullOrEmpty(dto.Address)) expert.Address = dto.Address;
                if (!string.IsNullOrEmpty(dto.City)) expert.City = dto.City;
                if (!string.IsNullOrEmpty(dto.State)) expert.State = dto.State;

                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.Information("Expert with ID: {Id} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update expert with ID: {Id}", id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Deleting expert with ID: {Id}", id);
            try
            {
                var expert = await _dbContext.Experts.FindAsync(id);
                if (expert == null)
                {
                    _logger.Warning("Expert with ID: {Id} not found for deletion", id);
                    return false;
                }

                _dbContext.Experts.Remove(expert);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.Information("Expert with ID: {Id} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete expert with ID: {Id}", id);
                return false;
            }
        }
    }

}
