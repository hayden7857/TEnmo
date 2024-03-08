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
        private static readonly Account ACCOUNT_1 = new Account(2001, 1001, 1000.00M);
        private static readonly Account ACCOUNT_2 = new Account(2002, 1002, 1000.00M);

        private TransferSqlDao dao;

        private Transfer testTransfer;

        [TestInitialize]
        public override void Setup()
        {
            dao = new TransferSqlDao(ConnectionString);
            testTransfer = new Transfer();
            base.Setup();
        }
    }


}
