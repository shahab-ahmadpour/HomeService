using App.Domain.Core.Skills.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Skills.Interfaces
{
    public interface ISkillRepository
    {
        Task<List<Skill>> GetAllAsync(CancellationToken cancellationToken);
        Task<Skill> GetByIdAsync(int skillId, CancellationToken cancellationToken);
        Task AddAsync(Skill skill, CancellationToken cancellationToken);
        Task UpdateAsync(Skill skill, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
