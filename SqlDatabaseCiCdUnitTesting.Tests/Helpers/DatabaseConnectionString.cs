using System;
using System.Configuration;

namespace SqlDatabaseCiCdUnitTesting.Tests.Helpers
{
    public static class DatabaseConnectionString
    {
        public static string GetDatabaseConnectionString()
        {
            var buildConfiguration = ConfigurationManager.AppSettings["BuildConfiguration"];
            var connectionString = ConfigurationManager.ConnectionStrings["ApplicationDatabase"].ConnectionString;
            var updatedConnectionString = connectionString.Replace(buildConfiguration, $"UnitTests_{UniqueDatabaseId}");
            return updatedConnectionString;
        }

        public static string UniqueDatabaseId { get; } = Guid.NewGuid().ToString().Replace("-", "_");
    }
}
