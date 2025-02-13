using App.Domain.Core.Skills.Entities;
using App.Domain.Core.Skills.Interfaces;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
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
        public SkillRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Skill>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Skills.ToListAsync(cancellationToken);
        }

        public async Task<Skill> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var skill = await _dbContext.Skills.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (skill == null)
            {
                throw new KeyNotFoundException($"Skill with ID {id} not found.");
            }
            return skill;
        }

        public async Task AddAsync(Skill skill, CancellationToken cancellationToken)
        {
            if (skill == null)
                throw new ArgumentNullException(nameof(skill));

            await _dbContext.Skills.AddAsync(skill, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Skill skill, CancellationToken cancellationToken)
        {
            if (skill == null)
                throw new ArgumentNullException(nameof(skill));

            _dbContext.Skills.Update(skill);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var skill = await GetByIdAsync(id, cancellationToken);
            if (skill != null)
            {
                _dbContext.Skills.Remove(skill);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
