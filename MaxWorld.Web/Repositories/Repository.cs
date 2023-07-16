using Dapper;
using System.Data;

namespace MaxWorld.Web.Repositories
{
    public class Repository
    {
        private IDbConnection _connection;

        public IDbTransaction? Transaction { get; set; }

        public Repository(IDbConnection connection)
        {
            _connection = connection;
        }

        public ManagedTransaction BeginTransaction()
        {
            if(Transaction != null)
            {
                throw new InvalidOperationException();
            }

            Transaction = _connection.BeginTransaction();
            return new ManagedTransaction(this);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            return _connection.QueryAsync<T>(sql, param, Transaction);
        }

        public Task<T> QueryFirstAsync<T>(string sql, object? param = null)
        {
            return _connection.QueryFirstAsync<T>(sql, param, Transaction);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
        {
            return _connection.QueryFirstOrDefaultAsync<T>(sql, param, Transaction);
        }

        public Task<int> ExecuteAsync(string sql, object? param = null)
        {
            return _connection.ExecuteAsync(sql, param, Transaction);
        }
    }
}
