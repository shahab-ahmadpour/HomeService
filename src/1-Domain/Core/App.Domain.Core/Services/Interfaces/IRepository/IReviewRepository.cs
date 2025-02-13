using App.Domain.Core._ِDTO.Reviews;
using App.Domain.Core.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Services.Interfaces.IRepository
{
    public interface IReviewRepository
    {
        Task<bool> CreateAsync(CreateReviewDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, UpdateReviewDto dto, CancellationToken cancellationToken);
        Task<ReviewDto> GetAsync(int id, CancellationToken cancellationToken);
        Task<List<ReviewDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    }
}
