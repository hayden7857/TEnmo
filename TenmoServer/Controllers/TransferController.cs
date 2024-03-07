using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
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
        public TransferController(IUserDao userDao, IAccountDao accountDao, ITransferDao transferDao)
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
            int fromAccountUserId = transfer.AccountFrom;
            int toAccountUserId = transfer.AccountTo;
            Account fromAccount = accountDao.GetAccountById(transfer.AccountFrom);
            Account toAccount = accountDao.GetAccountById(transfer.AccountTo);
            transfer.AccountFrom = fromAccount.AccountId;
            transfer.AccountTo = toAccount.AccountId;
            if (transfer.TransferTypeId == 2 && fromAccount.UserId != toAccount.UserId && fromAccount.Balance >= transfer.Amount && transfer.Amount > 0)
            {

                Transfer newTransfer = transferDao.CreateTransfer(transfer);
                Account senderAccount = accountDao.UpdateSenderAccount(newTransfer, fromAccount);
                Account receiverAccount = accountDao.UpdateReceiverAccount(newTransfer, toAccount);
                newTransfer.AccountFrom = fromAccountUserId;
                newTransfer.AccountTo = toAccountUserId;
                return Created($"/transfer/{newTransfer.TransferId}", newTransfer);
            }
            else if (transfer.TransferTypeId == 1 && fromAccount.UserId != toAccount.UserId && transfer.Amount > 0)
            {
                Transfer newTransfer = transferDao.CreateTransferRequest(transfer);
                newTransfer.AccountFrom = fromAccountUserId;
                newTransfer.AccountTo = toAccountUserId;
                return Created($"/transfer/{newTransfer.TransferId}", newTransfer);
            }
            return StatusCode(400, "Could not complete transfer.");

        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<Transfer>> GetTransfers()
        {
            IList<Transfer> transferList = new List<Transfer>();
            try
            {
                transferList = transferDao.ListCurrentUserTransfer(accountDao.GetAccountByUsername(User.Identity.Name));
                foreach (Transfer transfer in transferList)
                {
                    Account fromAccount = accountDao.GetAccountByAccountId(transfer.AccountFrom);
                    Account toAccount = accountDao.GetAccountByAccountId(transfer.AccountTo);
                    transfer.AccountTo = toAccount.UserId;
                    transfer.AccountFrom = fromAccount.UserId;
                }

            }
            catch (DaoException ex)
            {
                return StatusCode(400, "Could not get transfer list");
            }
            return Ok(transferList);
        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Transfer> GetTransfer(int id)
        {
            try
            {
                Transfer transfer = transferDao.GetTransferById(id);
                Account fromAccount = accountDao.GetAccountByAccountId(transfer.AccountFrom);
                Account toAccount = accountDao.GetAccountByAccountId(transfer.AccountTo);
                transfer.AccountTo = toAccount.UserId;
                transfer.AccountFrom = fromAccount.UserId;
                int userId = userDao.GetUserByUsername(User.Identity.Name).UserId;
                if (userId == transfer.AccountFrom || userId == transfer.AccountTo)
                {

                    return Ok(transfer);
                }
                else
                {
                    return Unauthorized("Unauthorized request.");
                }

            }
            catch (DaoException ex)
            {
                return NotFound();
            }

        }
    }
}
