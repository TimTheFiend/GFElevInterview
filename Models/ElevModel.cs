using GFElevInterview.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GFElevInterview.Models
{
    public class ElevModel
    {
        #region Database Tabel

        [Key]
        public string CPRNr { get; set; }

        [Display(Name = "Fornavn")]
        public string Fornavn { get; set; }

        public string Efternavn { get; set; }

        [Display(Name = "Udd. Linje")]
        public string UddLinje { get; set; }

        [Display(Name = "Udd. Adresse")]
        public string UddAdr { get; set; }

        [Display(Name = "Dansk Niveau")]
        public FagNiveau DanNiveau { get; set; }

        [Display(Name = "Engelsk Niveau")]
        public FagNiveau EngNiveau { get; set; }

        [Display(Name = "Matematik Niveau")]
        public FagNiveau MatNiveau { get; set; }

        /// <summary>
        /// Flag som indeholder status på eksamen og undervisning i samtlige fag.
        /// </summary>
        [Display(Name = "Merit status")]
        public Merit UddMerit { get; set; }

        [Display(Name = "Har SPS")]
        public bool? SPS { get; set; }

        [Display(Name = "Har EUD")]
        public bool? EUD { get; set; }

        [Display(Name = "EUV type")]
        public EUVType ElevType { get; set; }

        [Display(Name = "GF længde (uger)")]
        public int UddLængdeIUger { get; set; } = Tools.StandardVaerdier.MinimumUgerGF;

        #endregion Database Tabel

        #region Constructors

        public ElevModel() {
        }

        public ElevModel(string cprNr) {
            if (!cprNr.Contains('-')) {
                StringBuilder sb = new StringBuilder(cprNr, cprNr.Length + 1);
                cprNr = sb.Insert(6, '-').ToString();
            }
            this.CPRNr = cprNr;
        }

        public ElevModel(string cprNr, string fornavn, string efternavn) : this(cprNr) {
            this.Fornavn = fornavn;
            this.Efternavn = efternavn;
        }

        #endregion Constructors

        #region NotMapped properties (Eksamen & Undervisning

        [NotMapped]
        public bool? DanEksamen {
            get {
                if (DanNiveau == FagNiveau.Null) {
                    return null;
                }
                return UddMerit.HasFlag(Merit.DanskEksamen);
            }
        }

        [NotMapped]
        public bool? DanUndervisning {
            get {
                if (DanNiveau == FagNiveau.Null) {
                    return null;
                }
                return UddMerit.HasFlag(Merit.DanskUndervisning);
            }
        }

        [NotMapped]
        public bool? EngEksamen {
            get {
                if (DanNiveau == FagNiveau.Null) {
                    return null;
                }
                return UddMerit.HasFlag(Merit.EngelskEksamen);
            }
        }

        [NotMapped]
        public bool? EngUndervisning {
            get {
                if (DanNiveau == FagNiveau.Null) {
                    return null;
                }
                return UddMerit.HasFlag(Merit.EngelskUndervisning);
            }
        }

        [NotMapped]
        public bool? MatEksamen {
            get {
                if (DanNiveau == FagNiveau.Null) {
                    return null;
                }
                return UddMerit.HasFlag(Merit.MatematikEksamen);
            }
        }

        [NotMapped]
        public bool? MatUndervisning {
            get {
                if (DanNiveau == FagNiveau.Null) {
                    return null;
                }
                return UddMerit.HasFlag(Merit.MatematikUndervisning);
            }
        }

        #endregion NotMapped properties (Eksamen & Undervisning

        #region Get properties

        [NotMapped]
        public string FornavnEfternavn => $"{Fornavn} {Efternavn}";

        [NotMapped]
        public string EfternavnFornavn => $"{Efternavn}, {Fornavn}";

        /// <summary>
        /// Returnerer en lovlig string der kan bruges som filnavn.
        /// </summary>
        [NotMapped]
        public string FilnavnMerit => FilnavnBase + RessourceFil.endMerit;

        /// <summary>
        /// Returnerer en lovlig string der kan bruges som filnavn.
        /// </summary>
        [NotMapped]
        public string FilnavnRKV => FilnavnBase + RessourceFil.endRKV;

        /// <summary>
        /// Laver et lovligt filnavn til elevens blanket(ter).
        /// </summary>
        private string FilnavnBase {
            get {
                string filNavn = $"{CPRNr} - {EfternavnFornavn}";
                foreach (char invalidLetter in System.IO.Path.GetInvalidFileNameChars()) {
                    filNavn = filNavn.Replace(invalidLetter, '_');
                }
                return filNavn;
            }
        }

        #endregion Get properties

        /// <summary>
        /// Sætter eller fjerner flag status for et givent fag.
        /// </summary>
        /// <param name="fag">Faget der er tale om.</param>
        /// <param name="value"><c>true</c> hvis gennemført; ellers <c>false</c>.</param>
        public void SætMeritStatus(Merit fag, bool value) {
            switch (value) {
                case true:
                    UddMerit |= fag;
                    break;

                case false:
                    UddMerit &= ~fag;
                    break;
            }
        }

        /// <summary>
        /// Override af ToString()
        /// </summary>
        /// <returns>xxxxx-xxxx - Efternavn, Fornavn</returns>
        public override string ToString() {
            return $"({CPRNr}) - {EfternavnFornavn}";
        }

        /// <summary>
        /// Udregner hvor mange dage eleven IKKE skal være på grundforløb.
        /// </summary>
        [NotMapped]
        public int MeritLængdeIDage {
            get {
                if (CurrentElev.elev.ElevType == EUVType.EUV1) {
                    return 100; // Hele forløbet er merit
                }
                // Hvis merit i alle fag == (16 - 16 - 4) = -4 * -5 = 20; Altså 2 uger for hvert fag med merit
                // Hvis merit i ét fag == (18 - 16 - 4) = -2 * -5 = 10;
                // Hvis ingen merit == (20 - 16 - 4) = 0 * -5 = 0;
                return (UddLængdeIUger - Tools.StandardVaerdier.MinimumUgerGF - 4) * -5;
            }
        }

        /// <summary>
        /// Udregner og opdaterer Merit længde i uger hvor <see cref="UddLinje"/> bliver taget højde for.
        /// </summary>
        public void BeregnMeritIUger() {
            if (ElevType == EUVType.EUV1) {
                UddLængdeIUger = 0;
                return;
            }

            FagNiveau minNiveau = FagNiveau.F;
            int ekstraUger = 4;

            if (DanNiveau > minNiveau) {
                minNiveau = UddLinje == RessourceFil.itsupporter ? FagNiveau.E : FagNiveau.D;

                if (EngNiveau >= minNiveau) { ekstraUger -= 2; }
                if (MatNiveau >= minNiveau) { ekstraUger -= 2; }
            }

            UddLængdeIUger = Tools.StandardVaerdier.MinimumUgerGF + ekstraUger;
        }

        /// <summary>
        /// Returnerer en bool hvis eleven allerede har haft en samtale.
        /// </summary>
        public bool ErUdfyldt => DanNiveau != FagNiveau.Null || EngNiveau != FagNiveau.Null || MatNiveau != FagNiveau.Null;

        /// <summary>
        /// <c>true</c> hvis eleven er <25 år gammel; ellers <c>false</c>.
        /// </summary>
        public bool ErRKV {
            get {
                /// Forklaring på property
                ///Der kan ikke konverteres på samme linje som vi henter tallet via Substring
                ///Hvis sektionen starter med `0` vil den tage den næste position med værdien >0
                ///Hvilket vil sige at vi skal tage hver sektion som en string, og bagefter konverterer det til en int
                //NOTE: Det er kun nødvendigt at kigge på `år` til at starte med.
                //Det er først når der er tvivl i om vi kan sortere dem i RKV eller ej at vi behøver at kigge på dd/mm/yy

                int year = int.Parse(CPRNr.Substring(4, 2));

                //Er Elev åbenlyst mere end 25 år gammel?
                if (year < DateTime.Now.Year - 1900 - 25 && year > DateTime.Now.Year - 2000) {
                    ///Fx. 95 < 96[121 - 25] : Altså er personens fødselsår mindre end året for 25 år siden?
                    ///Dette sikre os at vi ikke tager folk der er født i slut-90erne
                    ///95 > 21 [2021 - 2000] : Dette gør at vi ikke tager 00 og frem med i regne stykket.
                    ///Alle der kommer herind er født imellem 1922 og 1995
                    return true;
                }

                int dag = int.Parse(CPRNr.ToString().Substring(0, 2));
                int måned = int.Parse(CPRNr.ToString().Substring(2, 2));

                // fra 0-99 til 19(00-99)
                year += 1900;
                if (!(year >= DateTime.Now.Year - 25)) {
                    //Eftersom vi allerede har sorteret alle der er fra før (CurrentYear - 25) fra, så kigger vi fra resten med >90 årstal
                    //Hvis tallet er mindre end 90, så er de fra efter 2000, og 100 skal tilføjes
                    year += 100;
                }

                //Hvis deres fødselsdag er mindre(før) eller lig med (de har fødselsdag i dag) så skal de have RKV.
                return new DateTime(year, måned, dag) <= DateTime.Now.AddYears(-25);
            }
        }
    }

    #region Enums

    /// <summary>
    /// Indeholder de forskellige EUV-typer.
    /// </summary>
    public enum EUVType
    {
        Null,
        EUV1,
        EUV2,
        EUV3
    }

    /// <summary>
    /// Indeholder fag niveauer der kan bruges til sammenligning.
    /// </summary>
    public enum FagNiveau
    {
        Null,
        G,
        F,
        E,
        D,
        C,
        B,
        A
    }

    /// <summary>
    /// Indeholder Dansk, Engelsk, og Matematiks værdier for Eksamen, og Undervisning.
    /// </summary>
    [Flags]
    public enum Merit
    {
        None = 0,
        DanskEksamen = 1,
        DanskUndervisning = 2,
        EngelskEksamen = 4,
        EngelskUndervisning = 8,
        MatematikEksamen = 16,
        MatematikUndervisning = 32
    }

    #endregion Enums
}