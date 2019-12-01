using Dapper;
using System.Collections.Generic;
using System.Data;

namespace SqlDatabaseCiCdUnitTesting.Tests.Helpers
{
    public class DapperWrapper
    {
        public int Execute(IDbConnection dbconnection, string sql, object param = null)
        {
            return dbconnection.Execute(sql, param);
        }

        public IEnumerable<T> Query<T>(IDbConnection dbConnection, string sql, object param = null)
        {
            return dbConnection.Query<T>(sql, param);
        }
    }
}
