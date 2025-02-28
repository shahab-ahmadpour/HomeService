using App.Domain.Core.DTO.Orders;
using App.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Services.Interfaces.IAppService
{
    public interface IOrderAppService
    {
        Task<bool> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken);
        Task<OrderDto> GetAsync(int id, CancellationToken cancellationToken);
        Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> UpdatePaymentStatusAsync(int id, PaymentStatus status, CancellationToken cancellationToken);
        Task<UpdateOrderDto> GetOrderForEditAsync(int id, CancellationToken cancellationToken);
        Task<List<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
    }
}
