using App.Domain.Core.DTO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Services.Interfaces.IService
{
    public interface IRequestService
    {
        Task<bool> CreateAsync(CreateRequestDto dto, CancellationToken cancellationToken);
        Task<List<RequestDto>> GetRequestsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
    }
}
