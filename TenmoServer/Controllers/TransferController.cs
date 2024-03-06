using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly IUserDao userDao;
        private readonly IAccountDao accountDao;
        private readonly ITransferDao transferDao;
        public TransferController(IUserDao userDao, IAccountDao accountDao,ITransferDao transferDao)
        {
            this.accountDao = accountDao;
            this.userDao = userDao;
            this.transferDao = transferDao;
        }
        [Authorize]
        [HttpPost]
        public ActionResult PostTransferOut(Transfer transfer)
        {
            User user = userDao.GetUserByUsername(User.Identity.Name);
            Account fromAccount = accountDao.GetAccountById(user.UserId);
            Account toAccount = accountDao.GetAccountById(transfer.AccountTo);
            transfer.AccountFrom = fromAccount.AccountId;
            transfer.AccountTo = toAccount.AccountId;
            Transfer newTransfer = transferDao.CreateTransfer(transfer);
            Account senderAccount = accountDao.UpdateSenderAccount(newTransfer, fromAccount);
            Account receiverAccount = accountDao.UpdateReceiverAccount(newTransfer, toAccount); 
            return Created($"/transfer/{newTransfer.TransferId}", newTransfer);
        }
    }
}
