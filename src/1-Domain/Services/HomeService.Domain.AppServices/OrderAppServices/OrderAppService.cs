using App.Domain.Core.DTO.Orders;
using App.Domain.Core.Enums;
using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.OrderAppServices
{
    public class OrderAppService : IOrderAppService
    {
        private readonly IOrderService _orderService;
        private readonly ILogger _logger;

        public OrderAppService(IOrderService orderService, ILogger logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Creating new order.");
            return await _orderService.CreateAsync(dto, cancellationToken);
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Updating order with Id: {Id}", id);
            var result = await _orderService.UpdateAsync(id, dto, cancellationToken);

            if (!result)
            {
                _logger.Warning("AppService: Failed to update order with Id: {Id}. Order not found.", id);
            }

            return result;
        }


        public async Task<OrderDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching order with Id: {Id}", id);
            return await _orderService.GetAsync(id, cancellationToken);
        }

        public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Fetching all orders.");
            return await _orderService.GetAllAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Deleting (Deactivating) order with Id: {Id}", id);
            return await _orderService.DeleteAsync(id, cancellationToken);
        }

        public async Task<bool> UpdatePaymentStatusAsync(int id, PaymentStatus status, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Updating payment status for order Id: {Id} to {Status}", id, status);
            return await _orderService.UpdatePaymentStatusAsync(id, status, cancellationToken);
        }

        public async Task<UpdateOrderDto> GetOrderForEditAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("AppService: Preparing order with Id: {Id} for editing.", id);

            var order = await _orderService.GetAsync(id, cancellationToken);
            if (order == null)
            {
                _logger.Warning("AppService: Order with Id: {Id} not found.", id);
                return null;
            }

            var dto = new UpdateOrderDto
            {
                Id = order.Id,
                PaymentStatus = order.PaymentStatus,
                IsActive = order.IsActive,
                CustomerName = order.CustomerName,
                ExpertName = order.ExpertName,
                RequestDescription = order.RequestDescription
            };

            _logger.Information("AppService: Order with Id: {Id} prepared for editing.", id);

            return dto;
        }

        public async Task<List<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching orders for CustomerId: {CustomerId} in OrderAppService", customerId);
            try
            {
                var orders = await _orderService.GetByCustomerIdAsync(customerId, cancellationToken);
                if (orders == null || !orders.Any())
                {
                    _logger.Warning("No orders retrieved for CustomerId: {CustomerId} in OrderAppService", customerId);
                }
                else
                {
                    _logger.Information("Successfully retrieved {OrderCount} orders for CustomerId: {CustomerId} in OrderAppService", orders.Count, customerId);
                }
                return orders;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching orders for CustomerId: {CustomerId} in OrderAppService", customerId);
                throw;
            }
        }

    }
}
