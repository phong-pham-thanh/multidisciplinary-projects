using Back_end.Data;
using Back_end.Mapper;
using Back_end.Models;

namespace Back_end.Repository
{
    public interface IUserRepository
    {
        public UsersModel GetUserLogin(string username, string password);

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
    }
}
