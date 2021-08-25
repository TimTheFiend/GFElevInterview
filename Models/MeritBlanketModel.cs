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

        private const int minUddannelsesLængdeIUger = 16;  // 16 uger er det laveste man kan have, altså standards-længden for alle.

        public int UddannelsesLængdeIUger { get; set; } = minUddannelsesLængdeIUger;

        /// <summary>
        /// Hvor mange dages merit <see cref="CurrentElev"/> får.
        /// </summary>
        public int MeritLængdeIDage {
            get {
                if (CurrentElev.elev.elevType == ElevType.EUV1) {
                    return 100;  // Hele forløbet er merit
                }
                int value = UddannelsesLængdeIUger - minUddannelsesLængdeIUger;
                return (value * 5) - 20;  //Uger * antal ugedage.
            }
        }

        public MeritBlanketModel() {
            Dansk = new FagModel();
            Engelsk = new FagModel();
            Matematik = new FagModel();
        }

        /// <summary>
        /// Beregner hvor mange ugers merit <see cref="CurrentElev"/> har, baseret på deres fag niveauer.
        /// </summary>
        /// <param name="elev">Eleven der bliver interviewet.</param>
        public void BeregnMeritIUger(ElevModel elev) {
            if (elev.elevType == ElevType.EUV1) {
                UddannelsesLængdeIUger = 0;
            }

            FagNiveau minNiveau = FagNiveau.F;
            int ekstraUger = 4;

            if (Dansk.Niveau > minNiveau) 
            {
                minNiveau = elev.uddannelse == config.AppSettings["itsupporter"] ? FagNiveau.E : FagNiveau.D;

                if (Engelsk.Niveau >= minNiveau) { ekstraUger -= 2; }
                if (Matematik.Niveau >= minNiveau) { ekstraUger -= 2; }
            }

            UddannelsesLængdeIUger += ekstraUger;
        }

        /// <summary>
        /// Returnerer en værdi baseret på om <see cref="MeritBlanketModel"/> er udfyldt.
        /// </summary>
        /// <value>
        ///   <c>true</c> hvis udfyldt; ellers, <c>false</c>.
        /// </value>
        public bool ErUdfyldt {
            get {
                if (Dansk.Niveau != FagNiveau.Null || Engelsk.Niveau != FagNiveau.Null || Matematik.Niveau != FagNiveau.Null) {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Returnerer en liste med mulige skoler baseret på fagniveau.
        /// </summary>
        /// <returns><see cref="List{T}"/> med navne på skole(r).</returns>
        public List<string> ValgmulighederSkoler() {
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
        /// Returnerer en liste med mulige uddannelser baseret på elevtype.
        /// </summary>
        /// <returns><see cref="List{T}"/> med "Ved Ikke" som den eneste forskel.</returns>
        public List<string> ValgmulighederUddannelser() {
            List<string> uddannelser = new List<string>() {
                config.AppSettings["infrastruktur"],
                config.AppSettings["itsupporter"],
                config.AppSettings["programmering"]
            };
            if (!CurrentElev.elev.ErRKV) {
                uddannelser.Add(config.AppSettings["vedIkke"]);
            }

            return uddannelser;
        }
    }
}