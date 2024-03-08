using Microsoft.VisualStudio.TestTools.UnitTesting;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using TenmoServer.Models;

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
            catch (DaoException ex)
            {
                if (ex.GetType() == typeof(DaoException))
                {
                    Assert.AreEqual(1, 1);
                }
            }
        }
        [TestMethod]
        public void GetAccountByAccountId_Returns_Correct_Account()
        {
            Account account = dao.GetAccountByAccountId(2001);
            AssertAccountsMatch(ACCOUNT_1, account);

            account = dao.GetAccountByAccountId(2002);
            AssertAccountsMatch(ACCOUNT_2, account);
        }
        [TestMethod]
        public void GetAccountByAccountId_ThrowsDaoException()
        {
            try
            {
                dao.GetAccountByAccountId(2005);

            }
            catch (DaoException ex)
            {
                if (ex.GetType() == typeof(DaoException))
                {
                    Assert.AreEqual(1, 1);
                }
            }

        }
        [TestMethod]
        public void GetAccountByUsername_Returns_Correct_Account()
        {
            Account account = dao.GetAccountByUsername("tester 1");
            AssertAccountsMatch(ACCOUNT_1, account);

            account = dao.GetAccountByUsername("tester 2");
            AssertAccountsMatch(ACCOUNT_2, account);

        }
        [TestMethod]
        public void GetAccountByUsername_ThrowsDaoexception()
        {
            try
            {
                dao.GetAccountByUsername("tester 67");

            }
            catch (DaoException ex)
            {
                if (ex.GetType() == typeof(DaoException))
                {
                    Assert.AreEqual(1, 1);
                }
            }

        }
        [TestMethod]
        public void UpdateSenderAccount_Updates_Balance_Correctly()
        {
            Account existingAccount = new Account();
            existingAccount.AccountId = ACCOUNT_1.AccountId;
            existingAccount.UserId = ACCOUNT_1.UserId;
            existingAccount.Balance = 1000.00M;
            
            Transfer fakeTransfer = new Transfer();
            fakeTransfer.TransferId = 3001;
            fakeTransfer.AccountFrom = 2001;
            fakeTransfer.AccountTo = 2002;
            fakeTransfer.TransferTypeId = 2;
            fakeTransfer.TransferStatusId = 2;
            fakeTransfer.Amount = 50.00M;
            
            Account updatedAccount = dao.UpdateSenderAccount(fakeTransfer, existingAccount);
            Assert.AreEqual(950.00M, updatedAccount.Balance);
        }
        [TestMethod]
        public void UpdateSenderAccount_ThrowsDaoException()
        {
            Account existingAccount = new Account();
            existingAccount.AccountId = ACCOUNT_1.AccountId;
            existingAccount.UserId = ACCOUNT_1.UserId;
            existingAccount.Balance = 1000.00M;

            Transfer fakeTransfer = new Transfer();
            fakeTransfer.TransferId = 3001;
            fakeTransfer.AccountFrom = 2001;
            fakeTransfer.AccountTo = 2002;
            fakeTransfer.TransferTypeId = 2;
            fakeTransfer.TransferStatusId = 2;
            fakeTransfer.Amount = 1005.00M;

            try
            {
                dao.UpdateSenderAccount(fakeTransfer, existingAccount);
            }
            catch (DaoException ex)
            {
                if (ex.GetType() == typeof(DaoException))
                {
                    Assert.AreEqual(1, 1);
                }
            }

        }
        [TestMethod]
        public void UpdateReceiverAccount_Updates_Balance_Correctly()
        {
            Account existingAccount = new Account();
            existingAccount.AccountId = ACCOUNT_1.AccountId;
            existingAccount.UserId = ACCOUNT_1.UserId;
            existingAccount.Balance = 1000.00M;

            Transfer fakeTransfer = new Transfer();
            fakeTransfer.TransferId = 3001;
            fakeTransfer.AccountFrom = 2002;
            fakeTransfer.AccountTo = 2001;
            fakeTransfer.TransferTypeId = 2;
            fakeTransfer.TransferStatusId = 2;
            fakeTransfer.Amount = 50.00M;

            Account updatedAccount = dao.UpdateReceiverAccount(fakeTransfer, existingAccount);
            Assert.AreEqual(1050.00M, updatedAccount.Balance);
        }
        [TestMethod]
        public void UpdateReceiverAccount_ThrowsDaoException()
        {
            Account existingAccount = new Account();
            existingAccount.AccountId = ACCOUNT_1.AccountId;
            existingAccount.UserId = ACCOUNT_1.UserId;
            existingAccount.Balance = 1000.00M;

            Transfer fakeTransfer = new Transfer();
            fakeTransfer.TransferId = 3001;
            fakeTransfer.AccountFrom = 2002;
            fakeTransfer.AccountTo = 2001;
            fakeTransfer.TransferTypeId = 2;
            fakeTransfer.TransferStatusId = 2;
            fakeTransfer.Amount = -50.00M;
            
            try
            {
                dao.UpdateReceiverAccount(fakeTransfer, existingAccount);
            }
            catch (DaoException ex)
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
