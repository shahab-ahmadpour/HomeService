using App.Domain.Core.DTO.Transactions;
using App.Domain.Core.Transactions.Entities;
using App.Domain.Core.Transactions.Interfaces.IRepository;
using App.Infrastructure.Db.SqlServer.Ef;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
        private readonly ILogger _logger;

        public TransactionRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(TransactionDto transactionDto, CancellationToken cancellationToken)
        {
            _logger.Information("Creating new transaction for OrderId: {OrderId}", transactionDto.OrderId);
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
                _logger.Information("Successfully created transaction for OrderId: {OrderId}", transactionDto.OrderId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to create transaction for OrderId: {OrderId}", transactionDto.OrderId);
                return false;
            }
        }

        public async Task<List<TransactionDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all transactions.");
            try
            {
                var transactions = await _dbContext.Transactions.AsNoTracking()
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

                _logger.Information("Fetched {Count} transactions.", transactions.Count);
                return transactions;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch all transactions.");
                throw;
            }
        }

        public async Task<TransactionDto> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching transaction with Id: {Id}", id);
            try
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

                if (transaction == null)
                {
                    _logger.Warning("Transaction with Id: {Id} not found.", id);
                }
                else
                {
                    _logger.Information("Fetched transaction with Id: {Id}", id);
                }

                return transaction;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to fetch transaction with Id: {Id}", id);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, TransactionDto transactionDto, CancellationToken cancellationToken)
        {
            _logger.Information("Updating transaction with Id: {Id}", id);
            try
            {
                var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
                if (transaction == null) return false;

                transaction.PaymentStatus = transactionDto.PaymentStatus;
                transaction.Amount = transactionDto.Amount;

                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.Information("Successfully updated transaction with Id: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update transaction with Id: {Id}", id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            _logger.Information("Deleting transaction with Id: {Id}", id);
            try
            {
                var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
                if (transaction == null) return false;

                transaction.IsActive = false;
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.Information("Successfully deleted transaction with Id: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete transaction with Id: {Id}", id);
                return false;
            }
        }
    }

}
