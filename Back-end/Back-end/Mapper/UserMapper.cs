using Back_end.Models;

namespace Back_end.Mapper
{
    public interface IUserMapper
    {
        public void ToDataModel(Users efObject, UsersModel dmObject);
        public void ToDataModelOnlySetting(Users efObject, UsersModel dmObject);
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
            modelObject.TemperatureWarning = efObject.TemperatureWarning;
            modelObject.AutoRunFanWhenOverHeat = efObject.AutoRunFanWhenOverHeat;
            modelObject.WarningWhenOverHeat = efObject.WarningWhenOverHeat;
            return modelObject;
        }

        public void ToDataModel(Users efObject, UsersModel dmObject)
        {
            if(dmObject == null)
            {
                return;
            }
            efObject = new Users();
            efObject.Name = dmObject.Name;
            efObject.Username = dmObject.Username;
            efObject.Password = dmObject.Password;
            efObject.AutoRunFanWhenOverHeat = dmObject.AutoRunFanWhenOverHeat;
            efObject.WarningWhenOverHeat = dmObject.WarningWhenOverHeat;
            efObject.TemperatureWarning = dmObject.TemperatureWarning;
        }

        public void ToDataModelOnlySetting(Users efObject, UsersModel dmObject)
        {
            if (dmObject == null)
            {
                return;
            }
            efObject.AutoRunFanWhenOverHeat = dmObject.AutoRunFanWhenOverHeat;
            efObject.WarningWhenOverHeat = dmObject.WarningWhenOverHeat;
            efObject.TemperatureWarning = dmObject.TemperatureWarning;
        }

    }
}
