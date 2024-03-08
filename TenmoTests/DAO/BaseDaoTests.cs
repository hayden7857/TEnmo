using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace TenmoTests.Tests.DAO
{
    [TestClass]
    public class BaseDaoTests
    {
        private const string dbName = "TenmoTemp";

        private static string AdminConnectionString;
        protected static string ConnectionString;
        //protected static string InvalidConnectionString;

        private TransactionScope transaction;

        [AssemblyInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            SetConnectionStrings(dbName);

            string sql = File.ReadAllText("create_test_db.sql").Replace("test_db_name", dbName);

            using (SqlConnection conn = new SqlConnection(AdminConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.ExecuteNonQuery();
            }

            sql = File.ReadAllText("tenmo_test_data.sql");
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
            }

        }

        [AssemblyCleanup]
        public static void AfterAllTests()
        {
            // drop the temporary database
            string sql = File.ReadAllText("drop_test_db.sql").Replace("test_db_name", dbName);

            using (SqlConnection conn = new SqlConnection(AdminConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestInitialize]
        public virtual void Setup()
        {
            // Begin the transaction
            transaction = new TransactionScope();

        }

        [TestCleanup]
        public void Cleanup()
        {
            // Roll back the transaction
            transaction.Dispose();
        }

        //protected string DidNotThrowAnyExceptionForInvalidConnection(string methodName)
        //{
        //    return $"{methodName}() did not throw exception if unable to reach database.";
        //}

        //protected string DidNotThrowDaoExceptionForInvalidConnection(string methodName)
        //{
        //    return $"{methodName}() threw an exception, but it should throw DaoException if unable to reach database.";
        //}
        //protected string DidNotThrowAnyExceptionForDataIntegrity(string methodName)
        //{
        //    return $"{methodName}() did not throw exception if data integrity violation occurs";
        //}

        //protected string DidNotThrowDaoExceptionForDataIntegrity(string methodName)
        //{
        //    return $"{methodName}() threw an exception, but it should throw DaoException if data integrity violation occurs";
        //}

        //protected string ThrewNotImplementedException(string methodName)
        //{
        //    return $"{methodName} not implemented";
        //}



        private static void SetConnectionStrings(string defaultDbName)
        {
            string host = Environment.GetEnvironmentVariable("DB_HOST") ?? @".\SQLEXPRESS";
            string dbName = Environment.GetEnvironmentVariable("DB_DATABASE") ?? defaultDbName;
            string username = Environment.GetEnvironmentVariable("DB_USERNAME");
            string password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (username != null && password != null)
            {
                ConnectionString = $"Data Source={host};Initial Catalog={dbName};User Id={username};Password={password};";
            }
            else
            {
                ConnectionString = $"Data Source={host};Initial Catalog={dbName};Integrated Security=SSPI;";
            }
            AdminConnectionString = ConnectionString.Replace(dbName, "master");
            //InvalidConnectionString = ConnectionString.Replace(dbName, "invalidDB");
        }
    }
}
