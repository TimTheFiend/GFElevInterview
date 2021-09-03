using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using config = System.Configuration.ConfigurationManager;

namespace GFElevInterview.Models
{
    public class DbTools : DbContext
    {
        public DbTools()
        {
            //NOTE: `EnsureDeleted` skal kun bruges under development!
            //`EnsureDeleted` bliver brugt fordi vi gerne vil nulstille databasen mellem debugging sessioner.
            //this.Database.EnsureDeleted
            //NulstilDatabase();
            this.Database.EnsureCreated();  //Gør at vi sikre os at databasen eksisterer, ellers laver den databasen.
        }

        //Her nulstilles og genskabes databasen
        private void NulstilDatabase()
        {
            //SletFilerPåNulstil(config.AppSettings.Get();
            //SletFiler();
            //SletFiler(config.AppSettings.Get("endRKV"), config.AppSettings.Get("endMerit"));
            //this.Database.EnsureDeleted();
            this.Database.EnsureCreated();  //Gør at vi sikre os at databasen eksisterer, ellers laver den databasen.
        }

        //ORIGINAL
        private void SletFiler()
        {
            //string[] filEndelser = new string[] {
            //    config.AppSettings.Get("endMerit"),
            //    config.AppSettings.Get("endRKV")
            //};

            foreach (string filEndelse in new string[] {
                config.AppSettings.Get("endMerit"),
                config.AppSettings.Get("endRKV") })
            {
                foreach (string fil in Data.AdminTools.HentFiler(filEndelse))
                {
                    System.IO.File.Delete(fil);
                }
                //                string[] filer = Data.AdminTools.HentFiler(filEndelse);
                //foreach (string fil in filer)
                //{
                //    System.IO.File.Delete(fil);
                //}

            }

        }



        #region Database tabel

        // public DbSet<ElevModel> Elever { get; set; }
        public DbSet<ElevModel> Elever { get; set; }
        #endregion Database tabel

        //Bliver Kaldt Når Elever skal tilføjes til en tom database.
        private void TilføjEleverTilTomDatabase(List<ElevModel> nyElever)
        {
            //Tilføjer en liste af elever til databasen.
            Elever.AddRange(nyElever);
            //Elever er tilføjet
            SaveChanges();
        }
        //I TilføjEleverTilEksisterendeDatabase checker vi om de "nye elever" allerede eksister eller om de skal tilføjes.
        private void TilføjEleverTilEksisterendeDatabase(List<ElevModel> nyElever)
        {
            foreach (ElevModel elev in nyElever)
            {
                if (Elever.Where(x => x.cprNr == elev.cprNr).FirstOrDefault() == null)
                {
                    Elever.Add(elev);
                }
            }
            SaveChanges();
        }

        //Bruges til debug
        public void TilføjElever()
        {
            //NulstilDatabase();
            List<ElevModel> nyElever = new List<ElevModel>()
            {
                new ElevModel { cprNr = "5544332211", fornavn = "Havesaks", efternavn = "Baghave"},
                new ElevModel { cprNr = "2211334455", fornavn = "Blomsterkasse", efternavn = "Baghave"},
                new ElevModel { cprNr = "1122334455", fornavn = "Blomst", efternavn = "Forhave"}
            };
            //hvis der er elever i databasen så køres TilføjEleverTilEksisterendeDatabase, hvis ikke så køres TilføjEleverTilTomDatabase.
            if (Elever.Count() > 0)
            {
                TilføjEleverTilEksisterendeDatabase(nyElever);
            }
            else
            {
                TilføjEleverTilTomDatabase(nyElever);
            }
        }

        //Reele metode
        public void TilføjElever(List<ElevModel> nyElever)
        {
            if (Elever.Count() > 0)
            {
                TilføjEleverTilEksisterendeDatabase(nyElever);
            }
            else
            {
                TilføjEleverTilTomDatabase(nyElever);
            }


        }
        #region Required

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={System.Configuration.ConfigurationManager.AppSettings["db"]}");  //Database navn bliver indsat
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// NOTE! Bruges kun under development!
        /// Bruges til at fylde databasen med dummy-data, så vi har noget at arbejde med.
        /// </summary>
        /// <param name="modelBuilder">ikke noget vi behøver at tænke på.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<string> skoler = new List<string>()
            {
                config.AppSettings.Get("ballerup"),
                config.AppSettings.Get("lyngby"),
                config.AppSettings.Get("frederiksberg")
            };
            Random rng = new Random();

            modelBuilder.Entity<ElevModel>().HasData(
                new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0, skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                new ElevModel { cprNr = "0101901234", fornavn = "Johammer", efternavn = "Søm", uddannelseAdresse = skoler[rng.Next(0, skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
            //    new ElevModel { cprNr = "1111011254", fornavn = "Joakim1", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
            //    new ElevModel { cprNr = "0201952134", fornavn = "Joakim2", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0, skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
            //    //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
            //    //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
            //    //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
            //    //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
            //    //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
            //    //new ElevModel { cprNr = "1111001234", fornavn = "Joakim", efternavn = "Krugstrup", uddannelseAdresse = skoler[rng.Next(0,skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false },
                new ElevModel { cprNr = "2405054587", fornavn = "Victor", efternavn = "Gawron", uddannelseAdresse = skoler[rng.Next(0, skoler.Count)], sps = rng.NextDouble() > 0.5 ? true : false, eud = rng.NextDouble() > 0.5 ? true : false }

            );
        }

        #endregion
    }
}