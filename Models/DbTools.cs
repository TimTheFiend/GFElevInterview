using Microsoft.EntityFrameworkCore;

namespace GFElevInterview.Models
{
    public class DbTools : DbContext
    {
        public DbSet<Elev> elever { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(
                "Data Source=elevDB.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
