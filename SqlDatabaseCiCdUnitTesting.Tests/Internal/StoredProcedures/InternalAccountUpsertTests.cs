using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDatabaseCiCdUnitTesting.Tests.Helpers;
using System.Data.SqlClient;
using System.Linq;

namespace SqlDatabaseCiCdUnitTesting.Tests.Internal.StoredProcedures
{
    [TestClass]
    public class InternalAccountUpsertTests
    {
        private readonly string _dbConnection = DatabaseConnectionString.GetDatabaseConnectionString();
        private readonly DapperWrapper _dapperWrapper = new DapperWrapper();

        [TestMethod]
        public void AccountUpsert_WhenAccountDoesNotAlreadyExist_InsertsNewRecord()
        {
            //Arrange
            int recordId;

            //Act
            using (var sqlConnection = new SqlConnection(_dbConnection))
            {
                recordId = _dapperWrapper.Query<int>(sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
                    new
                    {
                        Id = 0,
                        Username = "cwalker",
                        FirstName = "Carey",
                        LastName = "Walker",
                        Email = "test@test.com",
                        StatusId = 1
                    }).Single();
            }

            //Assert
            Assert.IsTrue(recordId > 0);
        }

        [TestMethod]
        public void AccountUpsert_WhenAccountAlreadyExists_UpdatesRecord()
        {
            //Arrange
            int recordId;
            using (var sqlConnection = new SqlConnection(_dbConnection))
            {
                recordId = _dapperWrapper.Query<int>(sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
                    new
                    {
                        Id = 0,
                        Username = "cwalker1",
                        FirstName = "Carey1",
                        LastName = "Walker1",
                        Email = "test@test.com",
                        StatusId = 1
                    }).Single();
            }

            //Act
            using (var sqlConnection = new SqlConnection(_dbConnection))
            {
                _dapperWrapper.Execute(sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
                    new
                    {
                        Id = recordId,
                        Username = "cwalker2",
                        FirstName = "Carey2",
                        LastName = "Walker2",
                        Email = "test2@test.com",
                        StatusId = 2
                    });
            }

            //Assert
            Account account;
            using (var sqlConnection = new SqlConnection(_dbConnection))
            {
                account = _dapperWrapper.Query<Account>(sqlConnection, "exec internal.AccountSelect @Id",
                    new { Id = recordId }).Single();
            }
            Assert.IsNotNull(account);
            Assert.AreEqual(recordId, account.Id);
            Assert.AreEqual("cwalker2", account.Username);
            Assert.AreEqual("Carey2", account.FirstName);
            Assert.AreEqual("Walker2", account.LastName);
            Assert.AreEqual("test2@test.com", account.Email);
            Assert.AreEqual("Active", account.StatusTitle);
        }
    }
}
