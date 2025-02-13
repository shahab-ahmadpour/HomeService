using App.Domain.Core._ِDTO.Orders;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
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

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var order = new Order
                {
                    CustomerId = dto.CustomerId,
                    ExpertId = dto.ExpertId,
                    ProposalId = dto.ProposalId,
                    RequestId = dto.RequestId,
                    FinalPrice = dto.FinalPrice,
                    PaymentStatus = dto.PaymentStatus,
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.Orders.AddAsync(order, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) return false;

            order.PaymentStatus = dto.PaymentStatus;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<OrderDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .Where(o => o.Id == id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    ExpertId = o.ExpertId,
                    FinalPrice = o.FinalPrice,
                    PaymentStatus = o.PaymentStatus,
                    CreatedAt = o.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return order;
        }

        public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var orders = await _dbContext.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    ExpertId = o.ExpertId,
                    FinalPrice = o.FinalPrice,
                    PaymentStatus = o.PaymentStatus,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return orders;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) return false;

            order.IsActive = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
