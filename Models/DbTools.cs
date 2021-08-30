using Microsoft.EntityFrameworkCore;

namespace GFElevInterview.Models
{
    public class DbTools : DbContext
    {
        public DbTools() {
            //NOTE: `EnsureDeleted` skal kun bruges under development!
            //`EnsureDeleted` bliver brugt fordi vi gerne vil nulstille databasen mellem debugging sessioner.
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();  //Gør at vi sikre os at databasen eksisterer, ellers laver den databasen.
        }

        #region Database tabel

       // public DbSet<ElevModel> Elever { get; set; }
        public DbSet<ElevModel> Elever { get; set; }
        #endregion Database tabel

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite($"Data Source={System.Configuration.ConfigurationManager.AppSettings["db"]}");  //Database navn bliver indsat
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// NOTE! Bruges kun under development!
        /// Bruges til at fylde databasen med dummy-data, så vi har noget at arbejde med.
        /// </summary>
        /// <param name="modelBuilder">ikke noget vi behøver at tænke på.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ElevModel>().HasData(

                new ElevModel { cprNr = "1111931234", fornavn = "Joakim0", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "0101004321", fornavn = "Peder", efternavn = "Eriksen" },
                new ElevModel { cprNr = "0101011234", fornavn = "Annelise", efternavn = "Andkjær" },
                new ElevModel { cprNr = "0101954321", fornavn = "Peder", efternavn = "Eriksen" },
                new ElevModel { cprNr = "1202341233", fornavn = "Søm", efternavn = "Johammer" },
                new ElevModel { cprNr = "1151931234", fornavn = "Joakim1", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1181931234", fornavn = "Joakim2", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1191931234", fornavn = "Joakim3", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1141931234", fornavn = "Joakim4", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1152931234", fornavn = "Joakim5", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1181231234", fornavn = "Joakim6", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1191951234", fornavn = "Joakim7", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1141901234", fornavn = "Joakim8", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1141908234", fornavn = "Joakim9", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1131901234", fornavn = "Joakim10", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1141901934", fornavn = "Joakim11", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1101901234", fornavn = "Joakim12", efternavn = "Krugstrup" },
                new ElevModel { cprNr = "1101901934", fornavn = "Joakim13", efternavn = "Krugstrup" }
            );
        }
    }
}