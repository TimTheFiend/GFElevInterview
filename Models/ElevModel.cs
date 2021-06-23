using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

        public bool? SPS { get; set; }
        public bool? EUD { get; set; }

        //UddannelsesLængden starter på 16 uger.
        //Hvert fag hvor Elevens niveau ikke er tilstrækkelig nok tilføjer 2 uger til UddannelsesLængde;
        [NotMapped]
        public int UddannelsesLængde { get; set; } = 16;


        public void ForlængUddannelse()
        {
            int forlængelseAfUddanelse = 2;

            if (UddannelsesLængde < 20)
            {
                UddannelsesLængde += forlængelseAfUddanelse;
            }
        }

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
    }
}