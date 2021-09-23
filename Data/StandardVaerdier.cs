using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Data
{
    public static class StandardVaerdier
    {
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
    }
}