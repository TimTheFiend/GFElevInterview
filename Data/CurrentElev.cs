using System;
using System.Collections.Generic;
using System.Text;
using GFElevInterview.Models;
namespace GFElevInterview.Data
{
    public static class CurrentElev
    {
        public static ElevModel elev = new ElevModel();
        public static MeritBlanketModel meritBlanket = new MeritBlanketModel();


        /// <summary>
        /// Henter en værdi, baseret på om <see cref="CurrentElev"/> har et fornavn.
        /// </summary>
        /// <value>
        ///   <c>true</c> hvis der ikke er valgt en elev; ellers, <c>false</c>.
        /// </value>
        public static bool ValgtElev {
            //NOTE: Kan ikke tjekke hvis CPRNr er tomt, da den har en værdi 0 selv hvis den ikke bliver sat.
            get {
                return !String.IsNullOrEmpty(elev.Fornavn);
            }
        }

        //NOTE: Skal opdateres løbende som flere blanketter tilføjes til `CurrentElev`
        public static bool HasSetValues {
            get {
                if (meritBlanket.HasSetValue) {
                    return true;
                }
                return false;
            }
        }

    }
}
