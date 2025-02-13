using App.Domain.Core._ِDTO.Transactions;
using App.Domain.Core.Transactions.Entities;
using App.Domain.Core.Transactions.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.DbAccess.Repository.Ef.Repositories.Transactions
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _dbContext;

        public TransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(TransactionDto transactionDto, CancellationToken cancellationToken)
        {
            try
            {
                var transaction = new Transaction
                {
                    OrderId = transactionDto.OrderId,
                    Amount = transactionDto.Amount,
                    PaymentMethod = transactionDto.PaymentMethod,
                    PaymentStatus = transactionDto.PaymentStatus,
                    TransactionDate = transactionDto.TransactionDate
                };

                await _dbContext.Transactions.AddAsync(transaction, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<TransactionDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Transactions.AsNoTracking()
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    OrderId = t.OrderId,
                    Amount = t.Amount,
                    PaymentMethod = t.PaymentMethod,
                    PaymentStatus = t.PaymentStatus,
                    TransactionDate = t.TransactionDate
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<TransactionDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    OrderId = t.OrderId,
                    Amount = t.Amount,
                    PaymentMethod = t.PaymentMethod,
                    PaymentStatus = t.PaymentStatus,
                    TransactionDate = t.TransactionDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            return transaction;
        }

        public async Task<bool> UpdateAsync(int id, TransactionDto transactionDto, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null) return false;

            transaction.PaymentStatus = transactionDto.PaymentStatus;
            transaction.Amount = transactionDto.Amount;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null) return false;

            transaction.IsActive = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
