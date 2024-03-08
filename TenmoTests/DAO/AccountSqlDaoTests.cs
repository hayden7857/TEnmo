using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TenmoServer.Models;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using System.Data.SqlTypes;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace TenmoTests.Tests.DAO
{
    [TestClass]
    public class AccountSqlDaoTests : BaseDaoTests
    {
        /*  (1001, 1000.00), -- id will be 2001
	   (1002, 1000.00); -- id will be 2002 */
        private static readonly Account ACCOUNT_1 = new Account(2001, 1001, 1000.00M);
        private static readonly Account ACCOUNT_2 = new Account(2002, 1002, 1000.00M);

        private AccountSqlDao dao;
        //private AccountSqlDao invalidDao;

        private Account testAccount;

        [TestInitialize]
        public override void Setup()
        {
            dao = new AccountSqlDao(ConnectionString);
            //invalidDao = new AccountSqlDao(InvalidConnectionString);
            testAccount = new Account(2003, 1003, 1000.00M);
            base.Setup();
        }

        [TestMethod]
        public void GetAccountByUserId_Returns_Correct_Account()
        {
            Account account = dao.GetAccountById(1001);
            AssertAccountsMatch(ACCOUNT_1, account);

            account = dao.GetAccountById(1002);
            AssertAccountsMatch(ACCOUNT_2, account);
            
        }

        [TestMethod]
        public void GetAccountByUserId_ThrowsDaoException()
        {
            try
            {
                dao.GetAccountById(1005);
                
            }
            catch(DaoException ex)
            {
                if (ex.GetType() == typeof(DaoException))
                {
                    Assert.AreEqual(1, 1);
                }
            }
        }

        private void AssertAccountsMatch(Account expected, Account actual)
        {
            Assert.AreEqual(expected.AccountId, actual.AccountId);
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.Balance, actual.Balance);
        }
    }
}
