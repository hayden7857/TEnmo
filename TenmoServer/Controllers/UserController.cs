using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserDao userDao;
    }
    public UserController(IUserDao, userDao)
    {

    }
}
