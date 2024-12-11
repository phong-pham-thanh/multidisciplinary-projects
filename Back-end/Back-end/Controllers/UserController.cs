using Back_end.Models;
using Back_end.Service;
using Microsoft.AspNetCore.Mvc;

namespace Back_end.Controllers
{
    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [Route("login/")]
        [HttpPost]
        public UsersModel GetLoginUser([FromBody] LoginModel data)
        {
            string username = data.username;
            string password = data.password;

            UsersModel userResult = _userService.GetUserLogin(username, password);
            if (userResult != null)
            {
                HttpContext.Session.SetInt32("currentUserId", userResult.Id);
            }
            return userResult;
        }
    }
}
