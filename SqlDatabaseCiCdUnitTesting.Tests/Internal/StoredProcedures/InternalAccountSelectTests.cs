using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDatabaseCiCdUnitTesting.Tests.Helpers;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SqlDatabaseCiCdUnitTesting.Tests.Internal.StoredProcedures
{
    [TestClass]
    public class InternalAccountSelectTests
    {
        private readonly string _dbConnection = DatabaseConnectionString.GetDatabaseConnectionString();
        private readonly DapperAdapter _dapperAdapter = new DapperAdapter();
        private SqlConnection _sqlConnection;

        [TestInitialize]
        public void Initialise()
        {
            _sqlConnection = new SqlConnection(_dbConnection);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if(_sqlConnection != null)
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }
        }

        [TestMethod]
        public void AccountSelect_WhenCalledWithNullId_ReturnsAllRecords()
        {
            //Arrange
            int recordId;
            recordId = _dapperAdapter.Query<int>(_sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
                new
                {
                    Id = 0,
                    Username = "cwalker",
                    FirstName = "Carey",
                    LastName = "Walker",
                    Email = "test@test.com",
                    StatusId = 1
                }).Single();

            recordId = _dapperAdapter.Query<int>(_sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
                new
                {
                    Id = 0,
                    Username = "cwalker",
                    FirstName = "Carey",
                    LastName = "Walker",
                    Email = "test@test.com",
                    StatusId = 1
                }).Single();

            //Act
            var records = new List<dynamic>();
            records = _dapperAdapter.Query<dynamic>(_sqlConnection, "exec internal.AccountSelect").ToList();

            //Assert
            Assert.IsTrue(records.Count >= 2);
        }

        [TestMethod]
        public void AccountSelect_WhenCalledWithValidId_ReturnsRecordThatMatchesTheId()
        {
            //Arrange
            int recordId;
            recordId = _dapperAdapter.Query<int>(_sqlConnection, "exec internal.AccountUpsert @Id, @Username, @FirstName, @LastName, @Email, @StatusId",
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
            Account account;
            account = _dapperAdapter.Query<Account>(_sqlConnection, "exec internal.AccountSelect @Id",
                new { Id = recordId }).Single();

            //Assert
            Assert.IsNotNull(account);
            Assert.AreEqual(recordId, account.Id);
            Assert.AreEqual("cwalker1", account.Username);
            Assert.AreEqual("Carey1", account.FirstName);
            Assert.AreEqual("Walker1", account.LastName);
            Assert.AreEqual("test@test.com", account.Email);
            Assert.AreEqual("Pending", account.StatusTitle);
        }

    }
}
