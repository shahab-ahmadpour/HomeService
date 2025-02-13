using App.Domain.Core._ِDTO.Orders;
using App.Domain.Core.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Services.Interfaces.IRepository
{
    public interface IOrderRepository
    {
        Task<bool> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken);
        Task<OrderDto> GetAsync(int id, CancellationToken cancellationToken);
        Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    }
}
