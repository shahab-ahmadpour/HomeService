using App.Domain.Core.DTO.Orders;
using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.Users.Interfaces.IAppService;
using App.Domain.Core.Users.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.AppServices.CustomerAppServices
{
    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;

        public CustomerAppService(ICustomerService customerService, ILogger logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        public async Task<CustomerDto> GetByAppUserIdAsync(int appUserId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching customer for AppUserId: {AppUserId}", appUserId);
            return await _customerService.GetByAppUserIdAsync(appUserId, cancellationToken);
        }

        public async Task<EditCustomerDto> GetEditCustomerProfileAsync(int appUserId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching edit customer profile for AppUserId: {AppUserId}", appUserId);
            var customer = await _customerService.GetByAppUserIdAsync(appUserId, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for AppUserId: {AppUserId}", appUserId);
                return null;
            }

            return new EditCustomerDto
            {
                AppUserId = customer.AppUserId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                ProfilePicture = customer.ProfilePicture,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                City = customer.City,
                State = customer.State
            };
        }

        public async Task<bool> UpdateCustomerProfileAsync(EditCustomerDto dto, CancellationToken cancellationToken)
        {
            _logger.Information("Updating customer profile for AppUserId: {AppUserId}", dto.AppUserId);
            var result = await _customerService.UpdateCustomerProfileAsync(dto, cancellationToken);
            if (result)
            {
                _logger.Information("Customer profile updated successfully for AppUserId: {AppUserId}", dto.AppUserId);
            }
            else
            {
                _logger.Warning("Failed to update customer profile for AppUserId: {AppUserId}", dto.AppUserId);
            }
            return result;
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching orders for CustomerId: {CustomerId}", customerId);
            return await _customerService.GetOrdersByCustomerIdAsync(customerId, cancellationToken);
        }

        public async Task<List<ProposalDto>> GetProposalsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching proposals for CustomerId: {CustomerId}", customerId);
            return await _customerService.GetProposalsByCustomerIdAsync(customerId, cancellationToken);
        }
    }
}
