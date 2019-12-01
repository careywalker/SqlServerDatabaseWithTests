using Microsoft.SqlServer.Dac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDatabaseCiCdUnitTesting.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDatabaseCiCdUnitTesting.Tests
{
    [TestClass]
    public class DatabaseSetupAndTeardown
    {
        private static readonly string MasterDbConnectionString = ConfigurationManager.ConnectionStrings["MasterDatabase"].ConnectionString;
        private static readonly string BuildConfiguration = ConfigurationManager.AppSettings["BuildConfiguration"];
        private static readonly string DatabaseName = $"SqlDatabaseCiCdUnitTesting_UniTests_{DatabaseConnectionString.UniqueDatabaseId}";

        [AssemblyInitialize()]
        public static void SetupDatabase(TestContext testContext)
        {
            var dacServices = new DacServices(MasterDbConnectionString);
            var dacpacPath = GetDacPacFilePath();

            var dacDeployOptions = new DacDeployOptions
            {
                CreateNewDatabase = true
            };

            dacDeployOptions.SqlCommandVariableValues.Add("Environment", "UnitTests");

            dacServices.Deploy(
                DacPackage.Load(dacpacPath),
                DatabaseName,
                true,
                options: dacDeployOptions
            );
        }

        [AssemblyCleanup()]
        public static void TearDown()
        {
            var offlineCommand = $"ALTER DATABASE {DatabaseName} SET OFFLINE WITH ROLBACK IMMEDIATE";
            var onlineCommand = $"ALTER DATABASE {DatabaseName} SET ONLINE";
            var dropCommand = $"DROP DATABASE {DatabaseName}";

            using(var sqlConnection = new SqlConnection(MasterDbConnectionString))
            {
                sqlConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = sqlConnection
                };

                sqlCommand.CommandText = offlineCommand;
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = onlineCommand;
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = dropCommand;
                sqlCommand.ExecuteNonQuery();
            }
        }

        private static string GetDacPacFilePath()
        {
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            var databaseProjectName = "SqlDatabaseCiCdUnitTesting";
            var databaseTestProjectName = "SqlDatabaseCiCdUnitTesting.Tests";
            return Path.Combine(currentPath.Replace(databaseTestProjectName, databaseProjectName).Replace(BuildConfiguration, "Output"), "SqlDatabaseCiCdUnitTesting.dacpac");
        }
    }
}
