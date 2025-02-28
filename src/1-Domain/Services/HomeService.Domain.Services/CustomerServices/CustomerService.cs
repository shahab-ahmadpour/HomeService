using App.Domain.Core.DTO.Orders;
using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.DTO.Users.AppUsers;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.Users.Interfaces.IRepository;
using App.Domain.Core.Users.Interfaces.IService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeService.Domain.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAppUserRepository _userRepository;
        private readonly ILogger _logger;

        public CustomerService(ICustomerRepository customerRepository, IAppUserRepository appUserRepository, ILogger logger)
        {
            _customerRepository = customerRepository;
            _userRepository = appUserRepository;
            _logger = logger;
        }

        public async Task<CustomerDto> GetByAppUserIdAsync(int appUserId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByAppUserIdAsync(appUserId, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("No customer found for AppUserId: {AppUserId}", appUserId);
                return null;
            }

            var user = await _userRepository.GetByIdAsync(appUserId, cancellationToken);
            if (user == null)
            {
                _logger.Warning("No AppUser found for AppUserId: {AppUserId}", appUserId);
                return null;
            }

            return new CustomerDto
            {
                Id = customer.Id,
                AppUserId = customer.AppUserId,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                AccountBalance = user.AccountBalance,
                IsEnabled = user.IsEnabled,
                IsConfirmed = user.IsConfirmed,
                CreatedAt = user.CreatedAt,
                Role = user.Role
            };
        }

        public async Task<bool> UpdateCustomerProfileAsync(EditCustomerDto dto, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByAppUserIdAsync(dto.AppUserId, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for update with AppUserId: {AppUserId}", dto.AppUserId);
                return false;
            }

            var user = await _userRepository.GetByIdAsync(dto.AppUserId, cancellationToken);
            if (user == null)
            {
                _logger.Warning("AppUser not found for update with AppUserId: {AppUserId}", dto.AppUserId);
                return false;
            }

            customer.PhoneNumber = dto.PhoneNumber;
            customer.Address = dto.Address;
            customer.City = dto.City;
            customer.State = dto.State;
            var customerUpdated = await _customerRepository.UpdateAsync(customer, cancellationToken);
            _logger.Information("Customer update result: {CustomerUpdated}", customerUpdated);

            var userDto = new UpdateAppUserDto
            {
                Id = dto.AppUserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ProfilePicture = dto.ProfilePicture,
                IsConfirmed = user.IsConfirmed,
                Role = user.Role,
                Password = null
            };
            var userUpdated = await _userRepository.UpdateAsync(dto.AppUserId, userDto, cancellationToken);
            _logger.Information("User update result: {UserUpdated}", userUpdated);

            if (customerUpdated && userUpdated)
            {
                _logger.Information("Profile updated successfully for AppUserId: {AppUserId}", dto.AppUserId);
                return true;
            }

            _logger.Warning("Failed to update profile for AppUserId: {AppUserId}", dto.AppUserId);
            return false;
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for CustomerId: {CustomerId}", customerId);
                return new List<OrderDto>();
            }

            _logger.Information("Orders count for CustomerId {CustomerId}: {OrdersCount}", customerId, customer.Orders.Count);
            return customer.Orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.AppUser.FirstName + " " + o.Customer.AppUser.LastName,
                ExpertId = o.ExpertId,
                ExpertName = o.Expert.AppUser.FirstName + " " + o.Expert.AppUser.LastName,
                RequestId = o.RequestId,
                RequestDescription = o.Request.Description,
                SubHomeServiceName = o.Request.SubHomeService.Name,
                ProposalId = o.ProposalId,
                Proposals = new List<ProposalDto> { new ProposalDto
        {
            Id = o.Proposal.Id,
            ExpertId = o.Proposal.ExpertId,
            ExpertName = o.Proposal.Expert.AppUser.FirstName + " " + o.Proposal.Expert.AppUser.LastName,
            OrderId = o.Id,
            RequestId = o.Proposal.RequestId,
            RequestDescription = o.Proposal.Request.Description,
            SkillId = o.Proposal.SkillId,
            Price = o.Proposal.Price,
            ExecutionDate = o.Proposal.ExecutionDate,
            Description = o.Proposal.Description,
            Status = o.Proposal.Status,
            ResponseTime = o.Proposal.ResponseTime,
            CreatedAt = o.Proposal.CreatedAt,
            IsEnabled = o.Proposal.IsEnabled
        }},
                FinalPrice = o.FinalPrice,
                PaymentStatus = o.PaymentStatus,
                IsActive = o.IsActive,
                CreatedAt = o.CreatedAt,
                OrderDate = o.CreatedAt,
                Status = o.PaymentStatus.ToString()
            }).ToList();
        }

        public async Task<List<ProposalDto>> GetProposalsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
            if (customer == null)
            {
                _logger.Warning("Customer not found for CustomerId: {CustomerId}", customerId);
                return new List<ProposalDto>();
            }

            return customer.Orders.Select(o => new ProposalDto
            {
                Id = o.Proposal.Id,
                ExpertId = o.Proposal.ExpertId,
                ExpertName = o.Proposal.Expert.AppUser.FirstName + " " + o.Proposal.Expert.AppUser.LastName,
                OrderId = o.Id,
                RequestId = o.Proposal.RequestId,
                RequestDescription = o.Proposal.Request.Description,
                SkillId = o.Proposal.SkillId,
                Price = o.Proposal.Price,
                ExecutionDate = o.Proposal.ExecutionDate,
                Description = o.Proposal.Description,
                Status = o.Proposal.Status,
                ResponseTime = o.Proposal.ResponseTime,
                CreatedAt = o.Proposal.CreatedAt,
                IsEnabled = o.Proposal.IsEnabled
            }).ToList();
        }
    }
}

