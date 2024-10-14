

using Microsoft.EntityFrameworkCore;
using Back_end.Models;

namespace Back_end.Data
{
    public class CoreContext:DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
    }
}
