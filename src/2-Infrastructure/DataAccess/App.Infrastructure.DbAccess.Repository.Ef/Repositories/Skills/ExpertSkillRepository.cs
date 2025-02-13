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
    public class ExpertSkillRepository : IExpertSkillRepository
    {
        private readonly AppDbContext _dbContext;
        public ExpertSkillRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ExpertSkill>> GetByExpertIdAsync(int expertId, CancellationToken cancellationToken)
        {
            return await _dbContext.ExpertSkills.Where(es => es.ExpertId == expertId).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(ExpertSkill expertSkill, CancellationToken cancellationToken)
        {
            if (expertSkill == null)
                throw new ArgumentNullException(nameof(expertSkill));

            await _dbContext.ExpertSkills.AddAsync(expertSkill, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int expertId, int skillId, CancellationToken cancellationToken)
        {
            var expertSkill = await _dbContext.ExpertSkills.FirstOrDefaultAsync(es => es.ExpertId == expertId && es.SkillId == skillId, cancellationToken);
            if (expertSkill != null)
            {
                _dbContext.ExpertSkills.Remove(expertSkill);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
