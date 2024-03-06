using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        Account GetAccountById(int id);
        Account GetAccountByUsername(string username);
        Account CreateAccount(User user);

    }
}
