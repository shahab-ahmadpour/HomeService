using App.Domain.Core.DTO.Users.AppUsers;
using App.Domain.Core.DTO.Users.Customers;
using App.Domain.Core.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Users.Interfaces.IRepository
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByAppUserIdAsync(int appUserId, CancellationToken cancellationToken);
        Task<Customer> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<CustomerDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
