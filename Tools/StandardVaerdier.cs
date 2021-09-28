using System;
using System.Collections.Generic;
using System.Linq;
using GFElevInterview.Models;

namespace GFElevInterview.Tools
{
    public static class StandardVaerdier
    {
        /// <summary>
        /// Returnerer <c>16</c>, da det er minimums længden på grundforløb.
        /// </summary>
        public static int MinimumUgerGF { get; } = int.Parse(RessourceFil.minimumGrundforløbLængde);

        public static string BallerupMerit => string.Format("{0} ({1})", RessourceFil.ballerup, RessourceFil.skoleMerit);

        public static string BallerupFuld => string.Format("{0} ({1})", RessourceFil.ballerup, RessourceFil.skoleIngenMerit);

        public static string SamletMeritFilnavn => RessourceFil.samletMerit.Substring(RessourceFil.samletMerit.LastIndexOf('\\') + 1);
        public static string SamletRKVFilNavn => RessourceFil.samletRKV.Substring(RessourceFil.samletRKV.LastIndexOf('\\') + 1);
      
         /// <summary>
        /// Henter en liste af unikkeskoler(-plus og merit).
        /// </summary>
        /// <returns>skoler</returns>
        public static List<string> HentUnikkeSkoler() {
            return new List<string>() {
                RessourceFil.ballerup,
                RessourceFil.frederiksberg,
                RessourceFil.lyngby
            };
        }

        /// <summary>
        /// Henter alle skoler med merit status.
        /// </summary>
        /// <param name="harMerit"> checker om den har merit: ja(true), nej(false)</param>
        /// <returns></returns>
        public static List<string> HentSkoler(bool harMerit) {
            if (harMerit) {
                return HentUnikkeSkoler();
            }
            return new List<string>() { RessourceFil.ballerup };
        }

        /// <summary>
        /// Henter skoler med merit og uden udfra navn og meritgivelse. 
        /// </summary>
        /// <returns>skoler</returns>
        public static List<string> HentAlleSkoler() {
            List<string> skoler = HentUnikkeSkoler();
            skoler.AddRange(new[] {
                BallerupMerit,
                BallerupFuld,
            });

            return skoler;
        }

        /// <summary>
        /// Returnerer uddannelser: "itsupport, infrastruktur, programmering og vedikke (hvis de ikke er EUV)".
        /// </summary>
        /// <param name="erEUV"></param>
        /// <returns></returns>
        public static List<string> HentUddannelser(bool erEUV) {
            List<string> uddannelser = new List<string>() {
                RessourceFil.itsupporter,
                RessourceFil.infrastruktur,
                RessourceFil.programmering
            };
            if (!erEUV) {
                uddannelser.Add(RessourceFil.vedIkke);
            }
            return uddannelser;
        }

        /// <summary>
        /// Returnerer arrayet "Ballerup, Frederiksberg, Lyngby, BalPlus, BalFuld".
        /// </summary>
        public static string[] HentSkoleDictKeys {
            get {
                //Ballerup, Frederiksberg, Lyngby, BalPlus, BalFuld
                return RessourceFil.skolerDictKey.Split(';');
            }
        }

        /// <summary>
        /// Tæller antallet af elever i hver skole (samt plus og fuld merit fra ballerup)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> HentCounterDict() {
            Dictionary<string, int> skoleAntal = new Dictionary<string, int>();

            foreach (string key in RessourceFil.skolerDictKey.Split(';')) {
                skoleAntal.Add(key, 0);
            }

            return skoleAntal;
        }

        public static List<string> HentFagNiveau() {
            return Enum.GetNames(typeof(FagNiveau)).Where(x => x != FagNiveau.Null.ToString()).ToList();
        }
    }
}