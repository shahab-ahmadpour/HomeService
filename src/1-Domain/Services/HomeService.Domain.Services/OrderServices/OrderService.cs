using App.Domain.Core.DTO.Orders;
using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.Enums;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Domain.Core.Services.Interfaces.IService;
using App.Domain.Core.Skills.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ILogger _logger;

        public OrderService(IOrderRepository orderRepository, ISkillRepository skillRepository, ILogger logger)
        {
            _orderRepository = orderRepository;
            _skillRepository = skillRepository;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Creating new order.");
            return await _orderRepository.CreateAsync(dto, cancellationToken);
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Updating order with Id: {Id}", id);
            return await _orderRepository.UpdateAsync(id, dto, cancellationToken);
        }

        public async Task<OrderDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Fetching order with Id: {Id}", id);
            return await _orderRepository.GetAsync(id, cancellationToken);
        }

        public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Service: Fetching all orders.");
            return await _orderRepository.GetAllAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Deleting (Deactivating) order with Id: {Id}", id);
            return await _orderRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<bool> UpdatePaymentStatusAsync(int id, PaymentStatus status, CancellationToken cancellationToken)
        {
            _logger.Information("Service: Updating payment status for order Id: {Id} to {Status}", id, status);
            return await _orderRepository.UpdatePaymentStatusAsync(id, status, cancellationToken);
        }

        public async Task<List<Order>> GetAllOrdersAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all orders asynchronously.");
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync(cancellationToken);
                _logger.Information("Fetched {Count} orders.", orders?.Count ?? 0);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch all orders asynchronously.");
                throw;
            }
        }

        public async Task<List<OrderDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching orders for CustomerId: {CustomerId} in OrderService", customerId);
            try
            {
                var orders = await _orderRepository.GetByCustomerIdAsync(customerId, cancellationToken);
                if (orders == null || !orders.Any())
                {
                    _logger.Warning("No orders found for CustomerId: {CustomerId} in OrderService", customerId);
                    return new List<OrderDto>();
                }

                var orderDtos = new List<OrderDto>();
                foreach (var o in orders)
                {
                    string subHomeServiceName = "نامشخص";
                    if (o.Proposal != null)
                    {
                        var skill = await _skillRepository.GetByIdAsync(o.Proposal.SkillId, cancellationToken);
                        subHomeServiceName = skill?.Name ?? "نامشخص";
                    }

                    string customerFullName = o.Customer?.AppUser != null
                        ? $"{o.Customer.AppUser.FirstName} {o.Customer.AppUser.LastName}".Trim()
                        : "نامشخص";
                    string expertFullName = o.Expert?.AppUser != null
                        ? $"{o.Expert.AppUser.FirstName} {o.Expert.AppUser.LastName}".Trim()
                        : "نامشخص";

                    orderDtos.Add(new OrderDto
                    {
                        Id = o.Id,
                        CustomerId = o.CustomerId,
                        CustomerName = customerFullName,
                        ExpertId = o.ExpertId,
                        ExpertName = expertFullName,
                        RequestId = o.RequestId,
                        RequestDescription = o.Request?.Description ?? "نامشخص",
                        SubHomeServiceName = subHomeServiceName,
                        ProposalId = o.ProposalId ?? 0,
                        Proposals = o.Proposal != null ? new List<ProposalDto>
                {
                    new ProposalDto
                    {
                        Id = o.Proposal.Id,
                        ExpertId = o.Proposal.ExpertId,
                        ExpertName = expertFullName,
                        RequestId = o.Proposal.RequestId,
                        RequestDescription = o.Request?.Description ?? "نامشخص",
                        SkillId = o.Proposal.SkillId,
                        Price = o.Proposal.Price,
                        ExecutionDate = o.Proposal.ExecutionDate,
                        Description = o.Proposal.Description,
                        Status = o.Proposal.Status,
                        ResponseTime = o.Proposal.ResponseTime,
                        CreatedAt = o.Proposal.CreatedAt,
                        IsEnabled = o.Proposal.IsEnabled
                    }
                } : new List<ProposalDto>(),
                        FinalPrice = o.FinalPrice,
                        PaymentStatus = o.PaymentStatus,
                        IsActive = o.IsActive,
                        CreatedAt = o.CreatedAt
                    });
                }

                _logger.Information("Successfully fetched {OrderCount} orders for CustomerId: {CustomerId} in OrderService", orderDtos.Count, customerId);
                return orderDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch orders for CustomerId: {CustomerId} in OrderService", customerId);
                throw;
            }
        }
        public async Task<Order> GetByProposalIdAsync(int proposalId, CancellationToken cancellationToken)
        {
            _logger.Information("OrderService: Fetching order by ProposalId: {ProposalId}", proposalId);
            var order = await _orderRepository.GetByProposalIdAsync(proposalId, cancellationToken);
            return order;
        }
    }
}
