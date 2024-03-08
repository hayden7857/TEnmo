using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer CreateTransfer(Transfer transfer);
        IList<Transfer> ListCurrentUserTransfer(Account user);
        Transfer GetTransferById(int id);
        Transfer CreateTransferRequest(Transfer transfer);
        IList<Transfer> ListCurrentUserPendingTransfers(Account user);
        Transfer UpdateTransfer(Transfer transfer);
    }
}
