using App.Domain.Core.DTO.Orders;
using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.DTO.Users.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Users.Interfaces.IAppService
{
    public interface ICustomerAppService
    {
        Task<CustomerDto> GetByAppUserIdAsync(int appUserId, CancellationToken cancellationToken);
        Task<EditCustomerDto> GetEditCustomerProfileAsync(int appUserId, CancellationToken cancellationToken);
        Task<bool> UpdateCustomerProfileAsync(EditCustomerDto dto, CancellationToken cancellationToken);
        Task<List<OrderDto>> GetOrdersByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<List<ProposalDto>> GetProposalsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
    }
}
