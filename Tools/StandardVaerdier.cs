using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Tools
{
    public static class StandardVaerdier
    {
        /// <summary>
        /// Returnerer <c>16</c>, da det er minimums længden på grundforløb.
        /// </summary>
        public static int MinimumUgerGF { get; } = Int32.Parse(RessourceFil.minimumGrundforløbLængde);

        public static List<string> HentUnikkeSkoler() {
            return new List<string>() {
                RessourceFil.ballerup,
                RessourceFil.frederiksberg,
                RessourceFil.lyngby
            };
        }

        public static List<string> HentSkoler(bool harMerit) {
            if (harMerit) {
                return HentUnikkeSkoler();
            }
            return new List<string>() { RessourceFil.ballerup };
        }

        public static List<string> HentAlleSkoler() {
            List<string> skoler = HentUnikkeSkoler();
            skoler.AddRange(new[] {
                string.Format("{0} ({1})", RessourceFil.ballerup, RessourceFil.skoleMerit),
                string.Format("{0} ({1})", RessourceFil.ballerup, RessourceFil.skoleIngenMerit),
            });

            return skoler;
        }

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

        public static List<string> HentUddannelserCmb()
        {
            return new List<string>() {
                RessourceFil.itsupporter,
                RessourceFil.infrastruktur,
                RessourceFil.programmering
            };
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

        public static Dictionary<string, int> HentCounterDict() {
            Dictionary<string, int> skoleAntal = new Dictionary<string, int>();

            foreach (string key in RessourceFil.skolerDictKey.Split(';')) {
                skoleAntal.Add(key, 0);
            }

            return skoleAntal;
        }
    }
}