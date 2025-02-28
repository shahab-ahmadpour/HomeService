using App.Domain.Core.DTO.Transactions;
using App.Domain.Core.Transactions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Transactions.Interfaces.IRepository
{
    public interface ITransactionRepository
    {
        Task<bool> CreateAsync(TransactionDto transactionDto, CancellationToken cancellationToken);
        Task<List<TransactionDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<TransactionDto> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(int id, TransactionDto transactionDto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
