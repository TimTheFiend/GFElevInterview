using GFElevInterview.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using config = System.Configuration.ConfigurationManager;

namespace GFElevInterview.Models
{
    public class _ElevModel
    {
        [Key]
        public string cprNr { get; set; }

        public string fornavn { get; set; }
        public string efternavn { get; set; }

        public string uddannelse { get; set; }
        public string udannelseAdresse { get; set; }

        public bool? sPS { get; set; }
        public bool? eUD { get; set; }

        public ElevType elevType { get; set; }

        public bool danskEksammen { get; set; }
        public bool danskUndervisning { get; set; }
        public FagNiveau danskNiveau { get; set; }

        public bool engelskEksammen { get; set; }
        public bool engelskUndervisning { get; set; }
        public FagNiveau engelskNiveau { get; set; }

        public bool matematikEksammen { get; set; }
        public bool matematikUndervisning { get; set; }
        public FagNiveau matematikNiveau { get; set; }

        //public FagModel Dansk;
        //public FagModel Engelsk;
        //public FagModel Matematik;

        #region FagSektion
        [NotMapped]
        public string danskPrintEksammen { get; set; }
        [NotMapped]
        public string danskPrintUndervisning { get; set; }
        [NotMapped]
        public string danskPrintNiveau { get; set; }

        [NotMapped]
        public string engelskPrintEksammen { get; set; }
        [NotMapped]
        public string engelskPrintUndervisning { get; set; }
        [NotMapped]
        public string engelskPrintNiveau { get; set; }

        [NotMapped]
        public string matematikPrintEksammen { get; set; }
        [NotMapped]
        public string matematikPrintUndervisning { get; set; }
        [NotMapped]
        public string matematikPrintNiveau { get; set; }
        #endregion

        public _ElevModel()
        {
            //Dansk = new FagModel();
            //Engelsk = new FagModel();
            //Matematik = new FagModel();
        }
        #region MeritSektion
        private const int minForløbslængdeIUger = 16;

        public int uddannelsesLængdeIUger { get; set; } = minForløbslængdeIUger;

        public int meritLængdeIDage
        {
            get
            {
                if (CurrentElev.elev.elevType == ElevType.EUV1)
                {
                    return 100; // Hele forløbet er merit
                }
                int value = uddannelsesLængdeIUger - minForløbslængdeIUger;
                return (value * 5) - 20; // Uger * antal ugedage.
            }
        }

        public void BeregnMeritIUger(_ElevModel elev)
        {
            if (elev.elevType == ElevType.EUV1)
            {
                uddannelsesLængdeIUger = 0;
            }

            FagNiveau minNiveau = FagNiveau.F;
            int ekstraUger = 4;

            if (danskNiveau > minNiveau)
            {
                minNiveau = elev.uddannelse == config.AppSettings["itsupporter"] ? FagNiveau.E : FagNiveau.D;

                if (engelskNiveau >= minNiveau) { ekstraUger -= 2; }
                if (matematikNiveau >= minNiveau) { ekstraUger -= 2; }
            }

            uddannelsesLængdeIUger += ekstraUger;
        }

        public bool ErUdfyldt
        {
            get
            {
                if (danskNiveau != FagNiveau.Null || engelskNiveau != FagNiveau.Null || matematikNiveau != FagNiveau.Null)
                {
                    return true;
                }
                return false;
            }
        }

        public List<string> ValgAfSkoler()
        {
            if (danskNiveau <= FagNiveau.F)
            {
                return new List<string>() {
                    config.AppSettings["ballerup"]
                };
            }
            return new List<string>() {
                config.AppSettings["frederiksberg"],
                config.AppSettings["lyngby"]
            };
        }


        public List<string> ValgAfUddannelser()
        {
            List<string> uddannelser = new List<string>() {
                config.AppSettings["infrastruktur"],
                config.AppSettings["itsupporter"],
                config.AppSettings["programmering"]
            };
            if (!CurrentElev.elev.erRKV)
            {
                uddannelser.Add(config.AppSettings["vedIkke"]);
            }

            return uddannelser;
        }
        #endregion

        [NotMapped]
        public string fornavnEfternavn
        {
            get { return $"{fornavn} {efternavn}"; }
        }

        /// <summary>
        /// Returnerer elevens formateret "Efternavn, Fornavn".
        /// </summary>
        [NotMapped]
        public string efternavnFornavn
        {
            get { return $"{efternavn}, {fornavn}"; }
        }

        /// <summary>
        /// Returnerer det formateret CPR-nr.
        /// </summary>
        public string cPRNr
        {
            get
            {
                // CprNr "XXXXXXXXX"
                StringBuilder sb = new StringBuilder(cprNr, cprNr.Length + 1);
                sb.Insert(6, '-');

                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return $"({cPRNr}) - {efternavnFornavn}";
        }

        ///NOTE: Unødvendig? Alt den gør er at returnerer <see cref="ToString"/>.
        public string FuldInfo
        {
            get { return this.ToString(); }
        }

        public bool erRKV
        {
            get
            {
                /// Forklaring på property
                ///Der kan ikke konverteres på samme linje som vi henter tallet via Substring
                ///Hvis sektionen starter med `0` vil den tage den næste position med værdien >0
                ///Hvilket vil sige at vi skal tage hver sektion som en string, og bagefter konverterer det til en int
                //NOTE: Det er kun nødvendigt at kigge på `år` til at starte med.
                //Det er først når der er tvivl i om vi kan sortere dem i RKV eller ej at vi behøver at kigge på dd/mm/yy
                string _år = cprNr.ToString().Substring(4, 2);
                int år = Int32.Parse(_år);

                //Er Elev åbenlyst ældre end 25?
                if (år < DateTime.Now.Year - 1900 - 25 && år > DateTime.Now.Year - 2000)
                {
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
                if (!(år >= DateTime.Now.Year - 25))
                {
                    //Eftersom vi allerede har sorteret alle der er fra før CurrentYear - 25 fra, så kigger vi fra resten med >90 årstal
                    //Hvis tallet er mindre end 90, så er de fra efter 2000, og 100 skal tilføjes
                    år += 100;
                }

                //Hvis deres fødselsdag er mindre(før) eller lig med (de har fødselsdag i dag) så skal de have RKV.
                if (new DateTime(år, måned, dag) <= DateTime.Now.AddYears(-25))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Returnerer en lovlig string der kan bruges som filnavn.
        /// </summary>
        public string FilNavn
        {
            get
            {
                string fileName = $"{cprNr} - {efternavnFornavn}";
                foreach (char invalidLetter in System.IO.Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(invalidLetter, '_');
                }
                return fileName + ".pdf";
            }
        }
    }
}
