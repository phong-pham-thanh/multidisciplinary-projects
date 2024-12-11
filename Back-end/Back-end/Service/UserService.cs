using Back_end.Models;
using Back_end.Repository;

namespace Back_end.Service
{
    public interface IUserService
    {
        public UsersModel GetUserLogin(string username, string password);

    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public UsersModel GetUserLogin(string username, string password)
        {
            return _repository.GetUserLogin(username, password);
        }
    }
}
