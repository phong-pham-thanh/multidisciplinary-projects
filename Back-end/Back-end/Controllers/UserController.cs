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
        private ISerialPortManager _serialPortManager;
        public UserController(IUserService userService, ISerialPortManager serialPortManager) 
        {
            _userService = userService;
            _serialPortManager = serialPortManager;
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
                _serialPortManager.SetCurrentUserId(userResult.Id);
                HttpContext.Session.SetInt32("currentUserId", userResult.Id);
            }
            return userResult;
        }

        [Route("test/")]
        [HttpGet]
        public UsersModel Test()
        {
            var temp = HttpContext.Session.GetInt32("currentUserId");
            return null;
        }

        [Route("getUserInfo/{id}")]
        [HttpGet]
        public UsersModel GetUserInfo(int id)
        {
            _serialPortManager.SetCurrentUserId(id);
            return _userService.GetUserById(id);
        }

        [Route("setSession/{userId}")]
        [HttpGet]
        public void SetSession(int userId)
        {
            _serialPortManager.SetCurrentUserId(userId);
            HttpContext.Session.SetInt32("currentUserId", userId);
            return;
        }
        [Route("setWarningTempurate")]
        [HttpPost]
        public bool SetWarningTempurate([FromBody] UsersModel saveObject)
        {
            int currentUserId = HttpContext.Session.GetInt32("currentUserId").Value;
            saveObject.Id = currentUserId;

            return _userService.SetWarningTemperature(saveObject);
        }
    }
}
