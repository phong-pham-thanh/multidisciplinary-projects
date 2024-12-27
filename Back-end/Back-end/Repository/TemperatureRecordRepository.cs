using Back_end.Data;
using Back_end.DataModel;
using Back_end.Mapper;
using Microsoft.EntityFrameworkCore.Internal;

namespace Back_end.Repository
{
    public interface ITemperatureRecordRepository
    {
        public bool AddNewRecord(int userId, int temperature);
    }
    public class TemperatureRecordRepository : ITemperatureRecordRepository
    {
        private readonly CoreContext _coreContext;

        public TemperatureRecordRepository(CoreContext context)
        {
            _coreContext = context;
        }
        public bool AddNewRecord(int userId, int temperature)
        {
            var connectionString = "Data Source=localhost;Initial Catalog=Multidisciplinary-Projects;Integrated Security=SSPI;MultipleActiveResultSets=True;Connection Timeout=180";
            using (var context = DbContextFactory.CreateDbContext(connectionString))
            {
                TemperatureRecord efObject = new TemperatureRecord();
                efObject.DateRecord = DateTime.Now;
                efObject.UserId = userId;
                efObject.Temperature = temperature;
                context.TemperatureRecord.Add(efObject);
                context.SaveChanges();
            }

            return true;
        }

    }
}
