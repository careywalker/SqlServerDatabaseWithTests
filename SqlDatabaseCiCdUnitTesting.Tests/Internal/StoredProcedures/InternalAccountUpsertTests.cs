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
        private readonly DapperAdapter _dapperWrapper = new DapperAdapter();
        private SqlConnection _sqlConnection;

        [TestInitialize]
        public void Initialise()
        {
            _sqlConnection = new SqlConnection(_dbConnection);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (_sqlConnection != null)
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }
        }

        [TestMethod]
        public void AccountUpsert_WhenAccountDoesNotAlreadyExist_InsertsNewRecord()
        {
            //Arrange
            int recordId;

            //Act
            recordId = _dapperWrapper.Query<int>(_sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
                new
                {
                    Id = 0,
                    Username = "cwalker",
                    FirstName = "Carey",
                    LastName = "Walker",
                    Email = "test@test.com",
                    StatusId = 1
                }).Single();

            //Assert
            Assert.IsTrue(recordId > 0);
        }

        [TestMethod]
        public void AccountUpsert_WhenAccountAlreadyExists_UpdatesRecord()
        {
            //Arrange
            int recordId;
            recordId = _dapperWrapper.Query<int>(_sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
                new
                {
                    Id = 0,
                    Username = "cwalker1",
                    FirstName = "Carey1",
                    LastName = "Walker1",
                    Email = "test@test.com",
                    StatusId = 1
                }).Single();

            //Act
            _dapperWrapper.Execute(_sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
                new
                {
                    Id = recordId,
                    Username = "cwalker2",
                    FirstName = "Carey2",
                    LastName = "Walker2",
                    Email = "test2@test.com",
                    StatusId = 2
                });

            //Assert
            Account account;
            account = _dapperWrapper.Query<Account>(_sqlConnection, "exec internal.AccountSelect @Id",
                new { Id = recordId }).Single();

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
