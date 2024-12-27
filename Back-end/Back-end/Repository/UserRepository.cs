using Back_end.Data;
using Back_end.Mapper;
using Back_end.Models;

namespace Back_end.Repository
{
    public interface IUserRepository
    {
        public UsersModel GetUserLogin(string username, string password);
        public UsersModel GetUserById(int id);
        public bool SetWarningTemperature(UsersModel saveObject);
        
    }
    public class UserRepository : IUserRepository
    {
        private readonly CoreContext _coreContext;
        private readonly IUserMapper _userMapper;

        public UserRepository(CoreContext context, IUserMapper userMapper) {
            _coreContext = context;
            _userMapper = userMapper;
        }


        public UsersModel GetUserLogin(string username, string password)
        {
            Users users = _coreContext.Users.Where(us => us.Username == username && us.Password == password).FirstOrDefault();
            return _userMapper.ToModel(users);
        }

        public UsersModel GetUserById(int id)
        {
            Users users = _coreContext.Users.Where(us => us.Id == id).FirstOrDefault();
            return _userMapper.ToModel(users);
        }
        public bool SetWarningTemperature(UsersModel saveObject)
        {
            Users efObject = _coreContext.Users.Where(u => u.Id == saveObject.Id).FirstOrDefault();

            _userMapper.ToDataModelOnlySetting(efObject, saveObject);
            _coreContext.SaveChanges();
            return true;
        }
    }
}
