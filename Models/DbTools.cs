using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GFElevInterview.Models
{
    public sealed class DbTools : DbContext
    {
        private static DbTools instance = new DbTools();
        public static DbTools Instance => instance;

        #region Database tabel

        // public DbSet<ElevModel> Elever { get; set; }
        public DbSet<ElevModel> Elever { get; set; }

        public DbSet<LoginModel> Login { get; set; }

        #endregion Database tabel

        public DbTools() {
            //NOTE: `EnsureDeleted` skal kun bruges under development!
            //`EnsureDeleted` bliver brugt fordi vi gerne vil nulstille databasen mellem debugging sessioner.
            //this.Database.EnsureDeleted();
            //NulstilDatabase();
            this.Database.EnsureCreated();  //Gør at vi sikre os at databasen eksisterer, ellers laver den databasen.
        }

        //Her nulstilles og genskabes databasen
        private void NulstilDatabase() {
            //SletFilerPåNulstil(config.AppSettings.Get();
            //SletFiler();
            //SletFiler(config.AppSettings.Get("endRKV"), config.AppSettings.Get("endMerit"));
            //this.Database.EnsureDeleted();
            this.Database.EnsureCreated();  //Gør at vi sikre os at databasen eksisterer, ellers laver den databasen.
        }

        //ORIGINAL
        private void SletFiler() {
            //string[] filEndelser = new string[] {
            //    config.AppSettings.Get("endMerit"),
            //    config.AppSettings.Get("endRKV")
            //};

            foreach (string filEndelse in new string[] {
                RessourceFil.endMerit,
                RessourceFil.endRKV}) {
                foreach (string fil in Data.AdminTools.HentFiler(filEndelse)) {
                    System.IO.File.Delete(fil);
                }
                //                string[] filer = Data.AdminTools.HentFiler(filEndelse);
                //foreach (string fil in filer)
                //{
                //    System.IO.File.Delete(fil);
                //}
            }
        }

        #region Gets

        public Dictionary<string, int> GetAntalEleverPerSkole() {
            Dictionary<string, int> skoleAntal = new Dictionary<string, int>();

            //Sæt det op så det er lettere at læse
            List<ElevModel> ballerup = Elever.Where(e => e.uddannelseAdresse == RessourceFil.ballerup).Select(e => e).ToList();

            int countBal = ballerup.Count;
            int countFre = Elever.Count(e => e.uddannelseAdresse == RessourceFil.frederiksberg);
            int countLyn = Elever.Count(e => e.uddannelseAdresse == RessourceFil.lyngby);

            int countBalPlus = ballerup.Count(e => e.danskNiveau > FagNiveau.F);
            int countBalFul = countBal - countBalPlus;

            //Tilføj til dictionary
            skoleAntal.Add(RessourceFil.ballerup, countBal);
            skoleAntal.Add(RessourceFil.frederiksberg, countFre);
            skoleAntal.Add(RessourceFil.lyngby, countLyn);

            skoleAntal.Add("balOrd", countBalPlus);
            skoleAntal.Add("balFul", countBalFul);

            return skoleAntal;
        }

        public List<ElevModel> VisAlle() {
            instance = new DbTools();
            return (from e in Elever
                    select e).ToList();
        }

        public List<ElevModel> VisSkole(string skole) {
            return (from e in Elever
                    where e.uddannelseAdresse == skole
                    select e).ToList();
        }

        public List<ElevModel> VisSkole(string skole, FagNiveau ekslusivNiveau, bool erNiveauHøjere) {
            if (erNiveauHøjere) {
                return (from e in Elever
                        where e.uddannelseAdresse == skole &&
                        e.danskNiveau > ekslusivNiveau
                        select e).ToList();
            }
            else {
                return (from e in Elever
                        where e.uddannelseAdresse == skole
                        && e.danskNiveau < ekslusivNiveau
                        && e.danskNiveau > FagNiveau.Null
                        select e).ToList();
            }
        }

        public List<ElevModel> VisSPS() {
            return (from e in Elever
                    where e.sps == true
                    select e).ToList();
        }

        public List<ElevModel> VisEUD() {
            return (from e in Elever
                    where e.eud == true
                    select e).ToList();
        }

        public List<ElevModel> VisRKV() {
            return (from e in Elever
                    where e.elevType != 0
                    select e).ToList();
        }

        public List<ElevModel> VisMerit() {
            return (from e in Elever
                    where e.danskNiveau > 0
                    select e).ToList();
        }

        #endregion Gets

        //Bliver Kaldt Når Elever skal tilføjes til en tom database.
        private void TilføjEleverTilTomDatabase(List<ElevModel> nyElever) {
            //Tilføjer en liste af elever til databasen.
            Elever.AddRange(nyElever);
            //Elever er tilføjet
            SaveChanges();
        }

        //I TilføjEleverTilEksisterendeDatabase checker vi om de "nye elever" allerede eksister eller om de skal tilføjes.
        private void TilføjEleverTilEksisterendeDatabase(List<ElevModel> nyElever) {
            foreach (ElevModel elev in nyElever) {
                if (Elever.Where(x => x.cprNr == elev.cprNr).FirstOrDefault() == null) {
                    Elever.Add(elev);
                }
            }
            SaveChanges();
        }

        //Bruges til debug
        public void TilføjElever() {
            //NulstilDatabase();
            List<ElevModel> nyElever = new List<ElevModel>() {
                //new ElevModel { cprNr = "5544332211", fornavn = "Havesaks", efternavn = "Baghave"},
                //new ElevModel { cprNr = "2211334455", fornavn = "Blomsterkasse", efternavn = "Baghave"},
                //new ElevModel { cprNr = "1122334455", fornavn = "Blomst", efternavn = "Forhave"}
            };
            //hvis der er elever i databasen så køres TilføjEleverTilEksisterendeDatabase, hvis ikke så køres TilføjEleverTilTomDatabase.
            if (Elever.Count() > 0) {
                TilføjEleverTilEksisterendeDatabase(nyElever);
            }
            else {
                TilføjEleverTilTomDatabase(nyElever);
            }
        }

        //Reele metode
        public void TilføjElever(List<ElevModel> nyElever) {
            if (Elever.Count() > 0) {
                TilføjEleverTilEksisterendeDatabase(nyElever);
            }
            else {
                TilføjEleverTilTomDatabase(nyElever);
            }
        }

        #region Required

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.UseSqlite($"Data Source={System.Configuration.ConfigurationManager.AppSettings["db"]}");  //Database navn bliver indsat
            optionsBuilder.UseSqlite($"Data Source={RessourceFil.db}");
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
                RessourceFil.ballerup,
                RessourceFil.lyngby,
                RessourceFil.frederiksberg
            };
            Random rng = new Random();

            modelBuilder.Entity<ElevModel>().HasData(
            new ElevModel("1203851123", "Johammer", "Søm"),
            new ElevModel("1103891245", "Eriksen", "Svend"),
            new ElevModel("123456-4321", "Samuel", "Jackson"),
            new ElevModel("2012009856", "Spacejam", "Michael Jordan"),
            new ElevModel("111193-1234", "Joakim", "Krugstrup")
            );

            modelBuilder.Entity<LoginModel>().HasData(
                new LoginModel().CreateInitialLogin()
                );
        }

        #endregion Required
    }
}