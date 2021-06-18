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
                new ElevModel { CprNr = 1202341233, Fornavn = "Søm", Efternavn = "Johammer" } ,
                new ElevModel { CprNr = 1151931234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1181931234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1191931234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1141931234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1152931234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1181231234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1191951234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1141901234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1141908234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1131901234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1141901934, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1101901234, Fornavn = "Joakim", Efternavn = "Krugstrup" },
                new ElevModel { CprNr = 1101901934, Fornavn = "Joakim", Efternavn = "Krugstrup" }

            );
        }
    }
}
