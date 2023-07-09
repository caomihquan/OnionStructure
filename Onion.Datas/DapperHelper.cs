using Dapper;
using Microsoft.Data.SqlClient;
using Onion.Datas.Abstract;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Onion.Datas
{
    public class DapperHelper : IDapperHelper
    {
        private readonly string _connectionString = string.Empty;

        public DapperHelper(IConfiguration connectionString)
        {
            _connectionString = connectionString.GetConnectionString("OnionConnection");
        }

        public async Task ExcuteNotReturn(string query, DynamicParameters parameters = null,IDbTransaction dbTransaction = null)
        {
            using (var dbConection = new SqlConnection(_connectionString))
            {
                await dbConection.ExecuteAsync(query, parameters, dbTransaction,commandType: CommandType.Text);
            }
        }

        public async Task<T> ExcuteReturn<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null)
        {
            using (var dbConection = new SqlConnection(_connectionString))
            {
                return (T)Convert.ChangeType(await dbConection.ExecuteScalarAsync<T>(query, parameters, dbTransaction, commandType: CommandType.Text),typeof(T));
            }
        }

        public async Task<IEnumerable<T>> ExcuteSqlReturnList<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null)
        {
            using (var dbConection = new SqlConnection(_connectionString))
            {
                return await dbConection.QueryAsync<T>(query, parameters, dbTransaction, commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<T>> ExcuteStoreProcedureReturnList<T>(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null)
        {
            using (var dbConection = new SqlConnection(_connectionString))
            {
                return await dbConection.QueryAsync<T>(query, parameters, dbTransaction, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
