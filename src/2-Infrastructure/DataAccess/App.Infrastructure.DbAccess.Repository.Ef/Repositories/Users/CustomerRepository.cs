using App.Domain.Core.DTO.Users.AppUsers;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.Users.Entities;
using App.Domain.Core.Users.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Users
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public CustomerRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Customer> GetByAppUserIdAsync(int appUserId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching customer by AppUserId: {AppUserId}", appUserId);
            try
            {
                var customer = await _dbContext.Customers
                    .FirstOrDefaultAsync(c => c.AppUserId == appUserId, cancellationToken);

                if (customer == null)
                    _logger.Warning("No customer found for AppUserId: {AppUserId}", appUserId);
                else
                    _logger.Information("Customer retrieved for AppUserId: {AppUserId}", appUserId);

                return customer;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch customer with AppUserId: {AppUserId}", appUserId);
                throw;
            }
        }

        public async Task<Customer> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .Include(c => c.AppUser)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Request)
                    .ThenInclude(r => r.SubHomeService)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Expert)
                        .ThenInclude(e => e.AppUser)
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Proposal)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            _logger.Information("Fetched customer with Id: {CustomerId}, Orders count: {OrdersCount}", id, customer?.Orders?.Count ?? 0);
            return customer;
        }

        public async Task<List<CustomerDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Customers
                .Include(c => c.AppUser)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    AppUserId = c.AppUserId,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    Email = c.AppUser.Email,
                    FirstName = c.AppUser.FirstName,
                    LastName = c.AppUser.LastName,
                    ProfilePicture = c.AppUser.ProfilePicture
                }).ToListAsync(cancellationToken);
        }

        public async Task<bool> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                AppUserId = dto.AppUserId,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                City = dto.City,
                State = dto.State
            };
            _dbContext.Customers.Add(customer);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken)
        {
            _dbContext.Customers.Update(customer);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var customer = await GetByIdAsync(id, cancellationToken);
            if (customer == null) return false;

            _dbContext.Customers.Remove(customer);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }

}
