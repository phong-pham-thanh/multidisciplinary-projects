using Back_end.Data;
using Microsoft.EntityFrameworkCore;

public static class DbContextFactory
{
    public static CoreContext CreateDbContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoreContext>();
        optionsBuilder.UseSqlServer(connectionString); // Hoặc bất kỳ provider nào bạn đang dùng
        return new CoreContext(optionsBuilder.Options);
    }
}
