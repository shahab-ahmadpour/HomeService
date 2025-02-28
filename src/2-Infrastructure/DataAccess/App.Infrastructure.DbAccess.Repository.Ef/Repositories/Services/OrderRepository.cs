using App.Domain.Core.DTO.Orders;
using App.Domain.Core.Enums;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public OrderRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Creating new order for Request ID: {RequestId}", dto.RequestId);
            try
            {
                var order = new Order
                {
                    CustomerId = dto.CustomerId,
                    ExpertId = dto.ExpertId,
                    RequestId = dto.RequestId,
                    ProposalId = dto.ProposalId,
                    FinalPrice = dto.FinalPrice,
                    PaymentStatus = dto.PaymentStatus,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.Orders.AddAsync(order, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.Information("Successfully created order for Request ID: {RequestId}", dto.RequestId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create order for Request ID: {RequestId}", dto.RequestId);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Updating order with ID: {Id}", id);

            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

            if (order == null)
            {
                _logger.Warning("Order with ID: {Id} not found", id);
                return false;
            }

            order.PaymentStatus = dto.PaymentStatus;
            order.IsActive = dto.IsActive;

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.Information("Successfully updated order with ID: {Id}", id);
            return true;
        }

        public async Task<List<Order>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching orders for CustomerId: {CustomerId}", customerId);
            try
            {
                var orders = await _dbContext.Orders
                    .Include(o => o.Request)
                    .Include(o => o.Expert)
                    .ThenInclude(e => e.AppUser)
                    .Include(o => o.Proposal)
                    .Where(o => o.CustomerId == customerId && o.IsActive)
                    .ToListAsync(cancellationToken);

                if (orders == null || !orders.Any())
                {
                    _logger.Warning("No active orders found for CustomerId: {CustomerId}", customerId);
                }
                else
                {
                    _logger.Information("Retrieved {OrderCount} active orders for CustomerId: {CustomerId}", orders.Count, customerId);
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch orders for CustomerId: {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<OrderDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching order with ID: {Id}", id);
            var order = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Expert)
                .Include(o => o.Request)
                .Where(o => o.Id == id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.AppUser.FirstName + " " + o.Customer.AppUser.LastName,
                    ExpertId = o.ExpertId,
                    ExpertName = o.Expert.AppUser.FirstName + " " + o.Expert.AppUser.LastName,
                    RequestId = o.RequestId,
                    RequestDescription = o.Request.Description,
                    FinalPrice = o.FinalPrice,
                    PaymentStatus = o.PaymentStatus,
                    IsActive = o.IsActive,
                    CreatedAt = o.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                _logger.Warning("Order with ID: {Id} not found", id);
            }
            return order;
        }

        public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all orders.");
            var orders = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Expert)
                .Include(o => o.Request)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.AppUser.FirstName + " " + o.Customer.AppUser.LastName,
                    ExpertId = o.ExpertId,
                    ExpertName = o.Expert.AppUser.FirstName + " " + o.Expert.AppUser.LastName,
                    RequestId = o.RequestId,
                    RequestDescription = o.Request.Description,
                    FinalPrice = o.FinalPrice,
                    PaymentStatus = o.PaymentStatus,
                    IsActive = o.IsActive,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync(cancellationToken);

            _logger.Information("Fetched {Count} orders.", orders.Count);
            return orders;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Deleting (deactivating) order with ID: {Id}", id);
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
                if (order == null)
                {
                    _logger.Warning("Order with ID: {Id} not found for deletion.", id);
                    return false;
                }

                order.IsActive = false;
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.Information("Successfully deactivated order with ID: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to deactivate order with ID: {Id}", id);
                return false;
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int id, PaymentStatus status, CancellationToken cancellationToken)
        {
            _logger.Information("Updating payment status for order ID: {Id} to {Status}", id, status);

            try
            {
                var order = await _dbContext.Orders
                    .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

                if (order == null)
                {
                    _logger.Warning("Order with ID: {Id} not found for payment status update.", id);
                    return false;
                }

                order.PaymentStatus = status;
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.Information("Successfully updated payment status for order ID: {Id} to {Status}", id, status);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update payment status for order ID: {Id}", id);
                return false;
            }
        }

        public List<Order> GetAllOrders()
        {
            return _dbContext.Orders.ToList();
        }

    }

}
