using Back_end.Models;

namespace Back_end.Mapper
{
    public interface IUserMapper
    {
        public UsersModel ToModel(Users efObject);

    }
    public class UserMapper : IUserMapper
    {
        public UserMapper() { }

        public UsersModel ToModel(Users efObject)
        {
            if (efObject == null)
            {
                return null;
            }

            UsersModel modelObject = new UsersModel();
            modelObject.Id = efObject.Id;
            modelObject.Name = efObject.Name;
            modelObject.Username = efObject.Username;
            modelObject.Password = efObject.Password;
            return modelObject;
        }
    }
}
