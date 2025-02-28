using App.Domain.Core.Skills.Entities;
using App.Domain.Core.Skills.Interfaces;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Skills
{
    public class SkillRepository : ISkillRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public SkillRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Skill>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all skills.");
            try
            {
                var skills = await _dbContext.Skills.ToListAsync(cancellationToken);
                _logger.Information("Fetched {Count} skills.", skills.Count);
                return skills;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch all skills.");
                throw;
            }
        }

        public async Task<Skill> GetByIdAsync(int skillId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching skill with ID: {SkillId}", skillId);
            try
            {
                var skill = await _dbContext.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillId, cancellationToken);
                if (skill == null)
                {
                    _logger.Warning("Skill with ID: {SkillId} not found", skillId);
                }
                return skill;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch skill with ID: {SkillId}", skillId);
                throw;
            }
        }

        public async Task AddAsync(Skill skill, CancellationToken cancellationToken)
        {
            _logger.Information("Adding new skill: {Name}", skill.Name);
            try
            {
                if (skill == null)
                    throw new ArgumentNullException(nameof(skill));

                await _dbContext.Skills.AddAsync(skill, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.Information("Successfully added skill with ID: {Id}", skill.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to add skill: {Name}", skill.Name);
                throw;
            }
        }

        public async Task UpdateAsync(Skill skill, CancellationToken cancellationToken)
        {
            _logger.Information("Updating skill with ID: {Id}", skill.Id);
            try
            {
                if (skill == null)
                    throw new ArgumentNullException(nameof(skill));

                _dbContext.Skills.Update(skill);
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.Information("Successfully updated skill with ID: {Id}", skill.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update skill with ID: {Id}", skill.Id);
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Deleting skill with ID: {Id}", id);
            try
            {
                var skill = await GetByIdAsync(id, cancellationToken);
                if (skill != null)
                {
                    _dbContext.Skills.Remove(skill);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    _logger.Information("Successfully deleted skill with ID: {Id}", id);
                }
                else
                {
                    _logger.Warning("Skill with ID: {Id} not found for deletion", id);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete skill with ID: {Id}", id);
                throw;
            }
        }
    }
}
