using App.Domain.Core.Users.Entities;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Users
{
    public class ExpertRepository
    {
        private readonly AppDbContext _context;

        public ExpertRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Expert> GetByIdAsync(int id)
        {
            var expert = await _context.Experts
                .Include(e => e.AppUser)
                .Include(e => e.ExpertSkills)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expert?.AppUser == null || expert?.ExpertSkills == null)
            {
                return null;
            }

            return expert;
        }


        public async Task<bool> CreateAsync(Expert expert)
        {
            _context.Experts.Add(expert);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Expert expert)
        {
            _context.Experts.Update(expert);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expert = await GetByIdAsync(id);
            if (expert == null) return false;

            expert.AppUser.IsEnabled = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Expert>> GetAllAsync()
        {
            return await _context.Experts.Include(e => e.AppUser).ToListAsync();
        }
    }

}
