using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using config = System.Configuration.ConfigurationManager;

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
            List<string> skoler = new List<string>()
            { 
                config.AppSettings.Get("ballerup"),
                config.AppSettings.Get("lyngby"),
                config.AppSettings.Get("frederiksberg")
            };
            Random rng = new Random();

            modelBuilder.Entity<ElevModel>().HasData(
                new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                new ElevModel { cprNr = "0101901234", fornavn = "Johammer", efternavn = "Søm", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                new ElevModel { cprNr = "2405054587", fornavn = "Victor", efternavn = "Gawron", uddannelseAdresse = skoler[rng.Next(0, skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false }

            );
        }
    }
}