using App.Domain.Core._ِDTO.Users.AppUsers;
using App.Domain.Core.Enums;
using App.Domain.Core.Users.Entities;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Users
{
    public class AppUserRepository
    {
        private readonly AppDbContext _context;

        public AppUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Customer)
                .Include(u => u.Expert)
                .Include(u => u.Admin)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }


        public async Task<bool> CreateAsync(AppUser appUser)
        {
            _context.Users.Add(appUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(AppUser appUser)
        {
            _context.Users.Update(appUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appUser = await GetByIdAsync(id);
            if (appUser == null) return false;

            appUser.IsEnabled = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }

}
