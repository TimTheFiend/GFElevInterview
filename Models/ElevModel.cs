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
        public string cprNr { get; set; }

        public string fornavn { get; set; }
        public string efternavn { get; set; }

        public string uddannelse { get; set; }
        public string uddannelseAdresse { get; set; }

        public FagNiveau danskNiveau { get; set; }
        public FagNiveau engelskNiveau { get; set; }
        public FagNiveau matematikNiveau { get; set; }

        /// <summary>
        /// Flag som indeholder status på eksamen og undervisning i samtlige fag.
        /// </summary>
        public Merit uddannelsesMerit { get; set; }

        public bool? sps { get; set; }
        public bool? eud { get; set; }

        public ElevType elevType { get; set; }

        public int uddannelsesLængdeIUger { get; set; } = Int32.Parse(RessourceFil.minimumGrundforløbLængde);

        #endregion Database Tabel

        #region Constructors

        public ElevModel() {
        }

        public ElevModel(string cprNr) {
            if (!cprNr.Contains('-')) {
                StringBuilder sb = new StringBuilder(cprNr, cprNr.Length + 1);
                cprNr = sb.Insert(6, '-').ToString();
            }
            this.cprNr = cprNr;
        }

        public ElevModel(string cprNr, string fornavn, string efternavn) : this(cprNr) {
            this.fornavn = fornavn;
            this.efternavn = efternavn;
        }

        #endregion Constructors

        #region NotMapped properties (Eksamen & Undervisning

        [NotMapped]
        public bool? danskEksamen {
            get {
                if (danskNiveau == FagNiveau.Null) {
                    return null;
                }
                return uddannelsesMerit.HasFlag(Merit.DanskEksamen);
            }
        }

        [NotMapped]
        public bool? danskUndervisning {
            get {
                if (danskNiveau == FagNiveau.Null) {
                    return null;
                }
                return uddannelsesMerit.HasFlag(Merit.DanskUndervisning);
            }
        }

        [NotMapped]
        public bool? engelskEksamen {
            get {
                if (danskNiveau == FagNiveau.Null) {
                    return null;
                }
                return uddannelsesMerit.HasFlag(Merit.EngelskEksamen);
            }
        }

        [NotMapped]
        public bool? engelskUndervisning {
            get {
                if (danskNiveau == FagNiveau.Null) {
                    return null;
                }
                return uddannelsesMerit.HasFlag(Merit.EngelskUndervisning);
            }
        }

        [NotMapped]
        public bool? matematikEksamen {
            get {
                if (danskNiveau == FagNiveau.Null) {
                    return null;
                }
                return uddannelsesMerit.HasFlag(Merit.MatematikEksamen);
            }
        }

        [NotMapped]
        public bool? matematikUndervisning {
            get {
                if (danskNiveau == FagNiveau.Null) {
                    return null;
                }
                return uddannelsesMerit.HasFlag(Merit.MatematikUndervisning);
            }
        }

        #endregion NotMapped properties (Eksamen & Undervisning

        #region Get properties

        [NotMapped]
        public string fornavnEfternavn {
            get { return $"{fornavn} {efternavn}"; }
        }

        [NotMapped]
        public string efternavnFornavn {
            get { return $"{efternavn}, {fornavn}"; }
        }

        /// <summary>
        /// Returnerer en lovlig string der kan bruges som filnavn.
        /// </summary>
        [NotMapped]
        public string MeritFilNavn {
            get {
                string fileName = $"{cprNr} - {efternavnFornavn}";
                foreach (char invalidLetter in System.IO.Path.GetInvalidFileNameChars()) {
                    fileName = fileName.Replace(invalidLetter, '_');
                }
                return fileName + RessourceFil.endMerit;
            }
        }

        /// <summary>
        /// Returnerer en lovlig string der kan bruges som filnavn.
        /// </summary>
        [NotMapped]
        public string RKVFilNavn {
            get {
                string fileName = $"{cprNr} - {efternavnFornavn}";
                foreach (char invalidLetter in System.IO.Path.GetInvalidFileNameChars()) {
                    fileName = fileName.Replace(invalidLetter, '_');
                }
                return fileName + RessourceFil.endRKV;
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
                    uddannelsesMerit |= fag;
                    break;

                case false:
                    uddannelsesMerit &= ~fag;
                    break;
            }
        }

        /// <summary>
        /// Override af ToString()
        /// </summary>
        /// <returns>xxxxx-xxxx - Efternavn, Fornavn</returns>
        public override string ToString() {
            return $"({cprNr}) - {efternavnFornavn}";
        }

        //TODO doku
        public int meritLængdeIDage {
            get {
                if (CurrentElev.elev.elevType == ElevType.EUV1) {
                    return 100; // Hele forløbet er merit
                }
                //TODO skriv bedre kommentar JOAKIM
                //Der bliver fjernet 4 uger fra udregningen da resultatet af det første minus-stykke vil være 0
                //ved at fjerne -4 fra resultatet gør vi at hvis man intet merit har går man i 0,
                //merit i et fag betyder 2, og 4 hvis i alt. gang det med 5 mens man udligner minus tallet
                //og man har uger i dage.
                return (uddannelsesLængdeIUger - Int32.Parse(RessourceFil.minimumGrundforløbLængde) - 4) * -5;
            }
        }

        //TODO doku
        public void BeregnMeritIUger() {
            if (elevType == ElevType.EUV1) {
                uddannelsesLængdeIUger = 0;
                return;
            }

            FagNiveau minNiveau = FagNiveau.F;
            int ekstraUger = 4;

            if (danskNiveau > minNiveau) {
                minNiveau = uddannelse == RessourceFil.itsupporter ? FagNiveau.E : FagNiveau.D;

                if (engelskNiveau >= minNiveau) { ekstraUger -= 2; }
                if (matematikNiveau >= minNiveau) { ekstraUger -= 2; }
            }

            uddannelsesLængdeIUger = Int32.Parse(RessourceFil.minimumGrundforløbLængde) + ekstraUger;
        }

        //TODO
        public bool ErUdfyldt {
            get {
                if (danskNiveau != FagNiveau.Null || engelskNiveau != FagNiveau.Null || matematikNiveau != FagNiveau.Null) {
                    return true;
                }
                return false;
            }
        }

        //TODO doku
        public bool erRKV {
            get {
                /// Forklaring på property
                ///Der kan ikke konverteres på samme linje som vi henter tallet via Substring
                ///Hvis sektionen starter med `0` vil den tage den næste position med værdien >0
                ///Hvilket vil sige at vi skal tage hver sektion som en string, og bagefter konverterer det til en int
                //NOTE: Det er kun nødvendigt at kigge på `år` til at starte med.
                //Det er først når der er tvivl i om vi kan sortere dem i RKV eller ej at vi behøver at kigge på dd/mm/yy
                string _år = cprNr.ToString().Substring(4, 2);
                int år = Int32.Parse(_år);

                //Er Elev åbenlyst ældre end 25?
                if (år < DateTime.Now.Year - 1900 - 25 && år > DateTime.Now.Year - 2000) {
                    ///Fx. 95 < 96[121 - 25] : Altså er personens fødselsår mindre end året for 25 år siden?
                    ///Dette sikre os at vi ikke tager folk der er født i slut-90erne
                    ///95 > 21 [2021 - 2000] : Dette gør at vi ikke tager 00 og frem med i regne stykket.
                    ///Alle der kommer herind er født imellem 1922 og 1995
                    return true;
                }

                string _dag = cprNr.ToString().Substring(0, 2);
                string _måned = cprNr.ToString().Substring(2, 2);

                int dag = Int32.Parse(_dag);
                int måned = Int32.Parse(_måned);

                // fra 0-99 til 19(00-99)
                år += 1900;
                if (!(år >= DateTime.Now.Year - 25)) {
                    //Eftersom vi allerede har sorteret alle der er fra før CurrentYear - 25 fra, så kigger vi fra resten med >90 årstal
                    //Hvis tallet er mindre end 90, så er de fra efter 2000, og 100 skal tilføjes
                    år += 100;
                }

                //Hvis deres fødselsdag er mindre(før) eller lig med (de har fødselsdag i dag) så skal de have RKV.
                if (new DateTime(år, måned, dag) <= DateTime.Now.AddYears(-25)) {
                    return true;
                }
                return false;
            }
        }
    }

    #region Enums

    /// <summary>
    /// Indeholder de forskellige EUV-typer.
    /// </summary>
    public enum ElevType
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