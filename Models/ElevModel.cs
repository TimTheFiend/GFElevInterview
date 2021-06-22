using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFElevInterview.Models
{
    public class ElevModel
    {
        [Key]
        public string CprNr { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }

        public string Uddannelse { get; set; }
        public string UdannelseAdresse { get; set; }
      
        public bool SPS { get; set; }
        public bool EUD { get; set; }

        [NotMapped]
        public string FornavnEfternavn {
            get { return $"{Fornavn} {Efternavn}"; }
        }

        [NotMapped]
        public string EfternavnFornavn {
            get { return $"{Efternavn}, {Fornavn}"; }
        }

        public override string ToString() {
            return $"({CprNr}) - {EfternavnFornavn}";
        }

        public string FullInfo {

            get { return this.ToString(); }

        }

        public bool IsRKV
        {
            get
            {
                /// Forklaring på property
                ///Der kan ikke konverteres på samme linje som vi henter tallet via Substring
                ///Hvis sektionen starter med `0` vil den tage den næste position med værdien >0
                ///Hvilket vil sige at vi skal tage hver sektion som en string, og bagefter konverterer det til en int
                //NOTE: Det er kun nødvendigt at kigge på `år` til at starte med.
                //Det er først når der er tvivl i om vi kan sortere dem i RKV eller ej at vi behøver at kigge på dd/mm/yy
                string _år = CprNr.ToString().Substring(4, 2);
                int år = Int32.Parse(_år);

                //Er Elev åbenlyst ældre end 25?
                if (år < DateTime.Now.Year - 1900 - 25 && år > DateTime.Now.Year - 2000) {
                    ///Fx. 95 < 96[121 - 25] : Altså er personens fødselsår mindre end året for 25 år siden?
                    ///Dette sikre os at vi ikke tager folk der er født i slut-90erne
                    ///95 > 21 [2021 - 2000] : Dette gør at vi ikke tager 00 og frem med i regne stykket.
                    ///Alle der kommer herind er født imellem 1922 og 1995 
                    return true;
                }

                string _dag = CprNr.ToString().Substring(0, 2);
                string _måned = CprNr.ToString().Substring(2, 2);

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

        public string FilNavn {
            get {
                string fileName = $"{CprNr} - {EfternavnFornavn}";
                foreach (char invalidLetter in System.IO.Path.GetInvalidFileNameChars()) {
                    fileName = fileName.Replace(invalidLetter, '_');
                }
                return fileName + ".pdf";
            }
        }
    }
}