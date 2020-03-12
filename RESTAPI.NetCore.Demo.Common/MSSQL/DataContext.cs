using Microsoft.EntityFrameworkCore;
using RESTAPI.NetCore.Demo.Common.Models;

namespace RESTAPI.NetCore.Demo.Common.MSSQL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DataContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Ignore(p => p.ObjectId);
        }

        public virtual DbSet<User> User { get; set; }
    }
}
