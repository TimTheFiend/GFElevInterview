using System.Collections.Generic;
using config = System.Configuration.ConfigurationManager;
using GFElevInterview.Data;


namespace GFElevInterview.Models
{
    public class MeritBlanketModel
    {
        public FagModel Dansk;
        public FagModel Engelsk;
        public FagModel Matematik;

        public int UddannelsesLængdeIUger { get; set; } = 16;  // 16 uger er det laveste man kan have, altså standarden.
        public int MeritLængdeIDage {
            get {
                if (CurrentElev.elev.ElevType == ElevType.EUV1) {
                    return 100;  // Hele forløbet er merit
                }
                int value = UddannelsesLængdeIUger - 16;  //
                return value * 5 - 20;  //Uger * antal ugedage. 
            }
        }
        public MeritBlanketModel() {
            Dansk = new FagModel();
            Engelsk = new FagModel();
            Matematik = new FagModel();
        }


        public void BeregnMeritIUger(ElevModel elev) {
            if (elev.ElevType == ElevType.EUV1) {
                UddannelsesLængdeIUger = 0;
            }

            FagNiveau minNiveau = FagNiveau.F;
            int ekstraUger = 4;

            if (Dansk.Niveau > minNiveau) {
                minNiveau = elev.Uddannelse == config.AppSettings["itsupporter"] ? FagNiveau.E : FagNiveau.D;

                if (Engelsk.Niveau >= minNiveau) { ekstraUger -= 2; }
                if (Matematik.Niveau >= minNiveau) { ekstraUger -= 2; }
            }

            UddannelsesLængdeIUger += ekstraUger;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is filled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is filled; otherwise, <c>false</c>.
        /// </value>
        public bool IsFilled {
            get {
                if (Dansk.Niveau != FagNiveau.Null || Engelsk.Niveau != FagNiveau.Null || Matematik.Niveau != FagNiveau.Null) {
                    return true;
                }
                return false;
            }
        }

        public List<string> AvailableSchools() {
            if (Dansk.Niveau <= FagNiveau.F) {
                return new List<string>() {
                    config.AppSettings["ballerup"]
                };
            }
            return new List<string>() {
                config.AppSettings["frederiksberg"],
                config.AppSettings["lyngby"]
            };
        }

        /// <summary>
        /// Viser hvilke uddannelser der er tilgængelig for eleven.
        /// </summary>
        public List<string> AvailableEducations() {
            List<string> uddannelser = new List<string>() {
                config.AppSettings["infrastruktur"],
                config.AppSettings["itsupporter"],
                config.AppSettings["programmering"]
            };
            if (!CurrentElev.elev.IsRKV) {
                uddannelser.Add(config.AppSettings["vedIkke"]);
            }

            return uddannelser;
        }
    }
}