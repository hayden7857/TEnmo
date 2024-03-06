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
    public class UserController : ControllerBase
    {
        private readonly IUserDao userDao;
        public UserController(IUserDao userDao)
        {
            this.userDao = userDao;
        }
        [Authorize]
        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            IList<User> userList = new List<User>();
            try
            {
                userList = userDao.GetUsers();
                
            }
            catch (DaoException ex)
            {
                return StatusCode(400,"Could not get user list");
            }
            return Ok(userList);
        }
    }
}
