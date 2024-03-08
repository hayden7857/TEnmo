using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoTests.Tests.DAO
{
    [TestClass]
    public class TransferSqlDaoTests : BaseDaoTests
    {
        /*  (1001, 1000.00), -- id will be 2001
	   (1002, 1000.00); -- id will be 2002 */
        private static readonly Transfer TRANSFER_1TransferOut= new Transfer(9999,2,2,2001,2002,50m);
        private static readonly Transfer TRANSFER_2TransferRequest= new Transfer(0, 1, 1, 2002, 2002, 50m);

        private TransferSqlDao dao;

        private Transfer testTransfer;

        [TestInitialize]
        public override void Setup()
        {
            dao = new TransferSqlDao(ConnectionString);
            testTransfer = new Transfer();
            base.Setup();
        }
        [TestMethod]
        public void CreateTransferCreatesATransfer()
        {
            dao.CreateTransfer(TRANSFER_1TransferOut);
            Transfer actual = dao.GetTransferById(9999);
            TransfersAreEqaul(TRANSFER_1TransferOut, actual);
            
        }
        public void TransfersAreEqaul(Transfer t1, Transfer t2)
        {
            Assert.AreEqual(t1.AccountFrom, t2.AccountFrom);
            Assert.AreEqual(t1.AccountTo, t2.AccountTo);
            Assert.AreEqual(t1.Amount, t2.Amount);
            Assert.AreEqual(t1.TransferId, t2.TransferId);
            Assert.AreEqual(t1.TransferStatusId, t2.TransferStatusId);
            Assert.AreEqual(t1.TransferTypeId, t2.TransferTypeId);

        }
    }


}
