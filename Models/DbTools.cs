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
        private DbSet<ElevModel> Elever { get; set; }

        public DbSet<LoginModel> Login { get; set; }

        #endregion Database tabel

        public DbTools() {
            //Gør at vi sikre os at databasen eksisterer, ellers laver den databasen.
            if (Database.EnsureCreated()) {
                Tools.AlertBoxes.OnEnsureCreatedDatabase();
            }
        }

        /// <summary>
        /// Tømmer <see cref="Elever"/> tabellen, og sletter alle individuelle blanketter i <see cref="RessourceFil.outputMappe"/>.
        /// </summary>
        public bool NulstilEleverAlt() {
            if (Tools.FilHandler.SletDokumenterIOutputMappe()) {
                Elever.RemoveRange(Elever);
                SaveChanges();

                Tools.AlertBoxes.OnFinishedInterview();
                return true;
            }
            return false;
        }

        public void OpdaterElevData(ElevModel elev) {
            Elever.Update(elev);
            SaveChanges();
        }

        public List<ElevModel> SearchElever(string brugerInput) {
            brugerInput = brugerInput.ToLower();

            return Elever.Where(e => e.cprNr.StartsWith(brugerInput)
                || e.fornavn.ToLower().StartsWith(brugerInput)
                || e.efternavn.ToLower().StartsWith(brugerInput)
            ).ToList();
        }

        #region Gets

        /// <summary>
        /// Laver en <see cref="Dictionary{TKey, TValue}"/> der indeholder en skole, og dens antal af elever.
        /// </summary>
        /// <returns>Skolens forkortede navn, samt antal.</returns>
        public Dictionary<string, int> GetAntalEleverPerSkole() {
            Dictionary<string, int> skoleAntal = new Dictionary<string, int>();
            string[] skoleKeys = Tools.StandardVaerdier.HentSkoleDictKeys;

            #region Hent antal af elever

            ///Ballerup har 3 kategorier: Total, Ordinær+, og Fuld,
            ///hvilket er grunden til at vi henter alle elever ind i en liste,
            ///for at kunne manipulere mere med dataen.
            List<ElevModel> ballerup = Elever.Where(e => e.uddannelseAdresse == RessourceFil.ballerup).Select(e => e).ToList();

            int countBal = ballerup.Count;
            int countBalPlus = ballerup.Count(e => e.danskNiveau > FagNiveau.F);  //Man er på Ordinær forløb hvis man har merit i dansk
            int countBalFul = countBal - countBalPlus;  //Fjerner elever på ordinært forløb fra det totale antal.

            int countFre = Elever.Count(e => e.uddannelseAdresse == RessourceFil.frederiksberg);
            int countLyn = Elever.Count(e => e.uddannelseAdresse == RessourceFil.lyngby);

            #endregion Hent antal af elever

            /* Tilføj til Dictionary */
            skoleAntal.Add(skoleKeys[0], countBal);
            skoleAntal.Add(skoleKeys[1], countFre);
            skoleAntal.Add(skoleKeys[2], countLyn);
            skoleAntal.Add(skoleKeys[3], countBalPlus);
            skoleAntal.Add(skoleKeys[4], countBalFul);

            return skoleAntal;
        }

        /// <summary>
        /// Henter alle <see cref="ElevModel"/> fra <see cref="Elever"/>.
        /// </summary>
        /// <returns></returns>
        public List<ElevModel> VisAlle() {
            instance = new DbTools();  //"Refresher" databasen hvis der er sket andre i en anden instans.

            //return Elever.Select(e => e).ToList();
            return (from e in Elever
                    select e).ToList();
        }

        /// <summary>
        /// Henter alle <see cref="ElevModel"/> fra <see cref="Elever"/> fra én skole.
        /// </summary>
        /// <param name="skoleNavn"></param>
        /// <returns></returns>
        public List<ElevModel> VisSkole(string skoleNavn) {
            //return Elever.Where(e => e.uddannelseAdresse == skoleNavn).Select(e => e).ToList();
            return (from e in Elever
                    where e.uddannelseAdresse == skoleNavn
                    select e).ToList();
        }

        /// <summary>
        /// Henter alle <see cref="ElevModel"/> fra <see cref="Elever"/> fra én skoles forløb.
        /// </summary>
        /// <param name="skole"></param>
        /// <param name="ekslusivNiveau"></param>
        /// <param name="erNiveauHøjere"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Henter alle <see cref="ElevModel"/> fra <see cref="Elever"/> hvor <see cref="ElevModel.sps"/> er <c>true</c>.
        /// </summary>
        /// <returns></returns>
        public List<ElevModel> VisSPS() {
            return (from e in Elever
                    where e.sps == true
                    select e).ToList();
        }

        /// <summary>
        /// Henter alle <see cref="ElevModel"/> fra <see cref="Elever"/> hvor <see cref="ElevModel.eud"/> er <c>true</c>.
        /// </summary>
        /// <returns></returns>
        public List<ElevModel> VisEUD() {
            return (from e in Elever
                    where e.eud == true
                    select e).ToList();
        }

        /// <summary>
        /// Henter alle <see cref="ElevModel"/> fra <see cref="Elever"/> som har en udskrevet RKV-blanket.
        /// </summary>
        /// <returns></returns>
        public List<ElevModel> VisRKV() {
            return (from e in Elever
                    where e.elevType != 0
                    select e).ToList();
        }

        /// <summary>
        /// Henter alle <see cref="ElevModel"/> fra <see cref="Elever"/> som har en udskrevet merit-blanket.
        /// </summary>
        /// <returns></returns>
        public List<ElevModel> VisMerit() {
            return (from e in Elever
                    where e.danskNiveau > 0
                    select e).ToList();
        }

        #endregion Gets

        /// <summary>
        /// Tilføjer udiskrimineret alle <see cref="ElevModel"/> objekter til <see cref="Elever"/>.
        /// </summary>
        /// <param name="nyElever"></param>
        private void TilføjEleverTilTomDatabase(List<ElevModel> nyElever) {
            Elever.AddRange(nyElever);
            SaveChanges();
        }

        /// <summary>
        /// Tilføjer alle unikke <see cref="ElevModel"/> objekter til <see cref="Elever"/>.
        /// </summary>
        /// <param name="nyElever"></param>
        private void TilføjEleverTilEksisterendeDatabase(List<ElevModel> nyElever) {
            foreach (ElevModel elev in nyElever) {
                if (Elever.Where(x => x.cprNr == elev.cprNr).FirstOrDefault() == null) {
                    Elever.Add(elev);
                }
            }
            SaveChanges();
        }

        /// <summary>
        /// Kalder <see cref="TilføjEleverTilTomDatabase(List{ElevModel})"/>,
        /// eller <see cref="TilføjEleverTilEksisterendeDatabase(List{ElevModel})"/>
        /// baseret på <see cref="Elever"/> status.
        /// </summary>
        /// <param name="nyElever">Elever der skal tilføjes til databasen.</param>
        public void TilføjElever(List<ElevModel> nyElever) {
            if (Elever.Count() > 0) {
                TilføjEleverTilEksisterendeDatabase(nyElever);
            }
            else {
                TilføjEleverTilTomDatabase(nyElever);
            }
        }

        #region DbContext.OnConfiguring, DbContext.OnModelCreating

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite($"Data Source={RessourceFil.db}");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Kaldes hvis databasen bliver *created*, efter ikke at eksisterer, og tilføjer én entry til <see cref="Login"/> tabellen.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //Sikre os at vi har tilføjet ét login når db bliver created.
            modelBuilder.Entity<LoginModel>().HasData(new LoginModel().CreateInitialLogin());
        }

        #endregion DbContext.OnConfiguring, DbContext.OnModelCreating
    }
}