using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TenmoServer.Models;
using TenmoServer.DAO;

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

        private Account testAccount;

        [TestInitialize]
        public override void Setup()
        {
            dao = new AccountSqlDao(ConnectionString);
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



        private void AssertAccountsMatch(Account expected, Account actual)
        {
            Assert.AreEqual(expected.AccountId, actual.AccountId);
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.Balance, actual.Balance);
        }
    }
}
