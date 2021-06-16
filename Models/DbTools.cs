using Microsoft.EntityFrameworkCore;

namespace GFElevInterview.Models
{
    public class DbTools : DbContext
    {
        public DbTools() {
            // If database already exists, load Elever tabel
            if (!Database.EnsureCreated()) {
                Elever.Load();
            }
        }

        public DbSet<ElevModel> Elever { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=elevDB.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        // Creates Dummy Data on creation
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ElevModel>().HasData(
                new ElevModel { CprNr = 1111931234, Fornavn = "Joakim", Efternavn = "Krugstrup" }, 
                new ElevModel { CprNr = 0101954321, Fornavn = "Peder", Efternavn = "Eriksen" }, 
                new ElevModel { CprNr = 1202341233, Fornavn = "Søm", Efternavn = "Johammer" } 
            );
        }
    }
}
