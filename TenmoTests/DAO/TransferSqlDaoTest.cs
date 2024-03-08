using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using TenmoServer.Models;

namespace TenmoTests.Tests.DAO
{
    [TestClass]
    public class TransferSqlDaoTests : BaseDaoTests
    {
        /*  (1001, 1000.00), -- id will be 2001
	   (1002, 1000.00); -- id will be 2002 */
        private static readonly Transfer TRANSFER_1TransferOut = new Transfer(9999, 2, 2, 2001, 2002, 50m);
        private static readonly Transfer TRANSFER_2TransferOut = new Transfer(9999, 2, 2, 2001, 2002, 50m);
        private static readonly Transfer TRANSFER_3TransferOut = new Transfer(9999, 2, 2, 2001, 2002, 50m);
        private static readonly Transfer TransferToSelf = new Transfer(9999, 2, 2, 2001, 2001, 50m);
        private static readonly Transfer TransferTooMuch = new Transfer(9999, 2, 2, 2001, 2002, decimal.MaxValue);
        private static readonly Transfer TRANSFER_2TransferRequest = new Transfer(0, 1, 1, 2002, 2001, 50m);


        private TransferSqlDao dao;
        private AccountSqlDao accountDao;
        private Transfer testTransfer;

        [TestInitialize]
        public override void Setup()
        {
            dao = new TransferSqlDao(ConnectionString);
            accountDao = new AccountSqlDao(ConnectionString);
            testTransfer = new Transfer();
            base.Setup();
        }
        [TestMethod]
        public void CreateTransferCreatesATransfer()
        {
            Transfer output = dao.CreateTransfer(TRANSFER_1TransferOut);
            Transfer actual = dao.GetTransferById(output.TransferId);
            TransfersAreEqaul(TRANSFER_1TransferOut, actual);

        }
        [TestMethod]
        public void CreateTransferToSelf()
        {
            try { Transfer output = dao.CreateTransfer(TransferToSelf); }
            catch (DaoException)
            {
                Assert.AreEqual(1, 1);
            }
        }
        [TestMethod]
        public void CreateTransferTooMuch()
        {
            try
            {
                dao.CreateTransfer(TransferTooMuch);
            }
            catch (DaoException)
            {
                Assert.AreEqual(1, 1);
            }
        }
        [TestMethod]
        public void CreateTransferRequestTest()
        {
            Transfer output = dao.CreateTransferRequest(TRANSFER_2TransferRequest);
            Transfer actual = dao.GetTransferById(output.TransferId);
            TransfersAreEqaul(TRANSFER_2TransferRequest, actual);
        }
        [TestMethod]
        public void ListCurrentUserTransferTest()
        {
            dao.CreateTransfer(TRANSFER_1TransferOut);
            dao.CreateTransfer(TRANSFER_2TransferOut);
            dao.CreateTransfer(TRANSFER_3TransferOut);
            IList<Transfer> actual = dao.ListCurrentUserTransfer(accountDao.GetAccountById(1001));
            Assert.AreEqual(actual.Count, 7);
        }
        [TestMethod]
        public void ListCurrentUserPendingTransfersTest()
        {
            IList<Transfer> actual = dao.ListCurrentUserPendingTransfers(accountDao.GetAccountById(1002));
            Assert.AreEqual(actual.Count, 1);
        }
        [TestMethod]
        public void UpdateTransferTest()
        {
            Transfer transfer1 = dao.GetTransferById(3001);
            transfer1.TransferStatusId = 3;
            dao.UpdateTransfer(transfer1);
            Transfer actual = dao.GetTransferById(3001);
            TransfersAreEqaul(transfer1, actual);
        }
        [TestMethod]
        public void TransfersAreEqaul(Transfer t1, Transfer t2)
        {
            Assert.AreEqual(t1.AccountFrom, t2.AccountFrom, "account from");
            Assert.AreEqual(t1.AccountTo, t2.AccountTo, "account to");
            Assert.AreEqual(t1.Amount, t2.Amount, "");
            Assert.AreEqual(t1.TransferStatusId, t2.TransferStatusId);
            Assert.AreEqual(t1.TransferTypeId, t2.TransferTypeId);

        }
    }


}
