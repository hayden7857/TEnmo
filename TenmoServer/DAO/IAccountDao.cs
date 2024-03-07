using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        Account GetAccountById(int id);
        Account GetAccountByAccountId(int id);
        Account GetAccountByUsername(string username);
        Account CreateAccount(User user);
        Account UpdateSenderAccount(Transfer transfer, Account account);
        Account UpdateReceiverAccount(Transfer transfer, Account account);

    }
}
