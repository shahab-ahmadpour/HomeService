using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Serilog;

namespace App.Infrastructure.DbAccess.Repository.Dapper
{
    public abstract class DapperRepositoryBase
    {
        protected readonly string _connectionString;
        protected readonly ILogger _logger;

        protected DapperRepositoryBase(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        protected IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            return connection;
        }

        protected async Task<IDbConnection> CreateOpenConnectionAsync()
        {
            var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            return connection;
        }

        protected async Task<T> ExecuteWithLoggingAsync<T>(string operationName, Func<Task<T>> operation)
        {
            _logger.Information("Repository (Dapper): Starting {OperationName}", operationName);
            try
            {
                var result = await operation();
                _logger.Information("Repository (Dapper): Completed {OperationName}", operationName);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Repository (Dapper): Error during {OperationName}", operationName);
                throw;
            }
        }
    }
}