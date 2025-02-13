using App.Domain.Core._ِDTO.Users.AppUsers;
using App.Domain.Core._ِDTO.Users.Customers;
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
    public class CustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                throw new Exception("customer not found");
            }

            return customer;
        }


        public async Task<bool> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await GetByIdAsync(id);
            if (customer == null) return false;

            customer.AppUser.IsEnabled = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.Include(c => c.AppUser).ToListAsync();
        }
    }

}
