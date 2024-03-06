using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Exceptions;
using TenmoServer.Models;


namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserDao userDao;
        private readonly IAccountDao accountDao;
        public AccountController(IUserDao userDao, IAccountDao accountDao)
        {
            this.accountDao = accountDao;
            this.userDao = userDao;
        }
        [Authorize]
        [HttpGet]
        public ActionResult<Account> GetAccount()
        {
            Account returnedAccount = new Account();
            User user;
            try
            {
                user = userDao.GetUserByUsername(User.Identity.Name);
                returnedAccount = accountDao.GetAccountById(user.UserId);

            }
            catch(DaoException ex)
            {
                return returnedAccount;
            }
            return Ok(returnedAccount);
        }
    }

}
