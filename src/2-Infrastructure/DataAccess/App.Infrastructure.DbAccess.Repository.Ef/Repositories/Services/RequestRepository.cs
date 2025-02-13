using App.Domain.Core._ِDTO.Requests;
using App.Domain.Core.Services.Entities;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Services
{
    public class RequestRepository : IRequestRepository
    {
        private readonly AppDbContext _dbContext;

        public RequestRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(CreateRequestDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var request = new Request
                {
                    CustomerId = dto.CustomerId,
                    SubHomeServiceId = dto.SubHomeServiceId,
                    Description = dto.Description,
                    Status = dto.Status,
                    Deadline = dto.Deadline,
                    ExecutionDate = dto.ExecutionDate,
                    EnvironmentImage = dto.EnvironmentImage,
                    CreatedAt = DateTime.UtcNow,
                    IsEnabled = true
                };

                await _dbContext.Requests.AddAsync(request, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateRequestDto dto, CancellationToken cancellationToken)
        {
            var request = await _dbContext.Requests.FindAsync(id);
            if (request == null) return false;

            request.Status = dto.Status;
            request.Deadline = dto.Deadline;
            request.ExecutionDate = dto.ExecutionDate;
            request.EnvironmentImage = dto.EnvironmentImage;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<RequestDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var request = await _dbContext.Requests
                .Where(r => r.Id == id && r.IsEnabled)
                .Select(r => new RequestDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    SubHomeServiceId = r.SubHomeServiceId,
                    Description = r.Description,
                    Status = r.Status,
                    Deadline = r.Deadline,
                    ExecutionDate = r.ExecutionDate,
                    EnvironmentImage = r.EnvironmentImage
                })
                .FirstOrDefaultAsync(cancellationToken);

            return request;
        }

        public async Task<List<RequestDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var requests = await _dbContext.Requests
                .Where(r => r.IsEnabled)
                .Select(r => new RequestDto
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    SubHomeServiceId = r.SubHomeServiceId,
                    Description = r.Description,
                    Status = r.Status,
                    Deadline = r.Deadline,
                    ExecutionDate = r.ExecutionDate,
                    EnvironmentImage = r.EnvironmentImage
                })
                .ToListAsync(cancellationToken);

            return requests;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var request = await _dbContext.Requests.FindAsync(id);
            if (request == null) return false;

            request.IsEnabled = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
