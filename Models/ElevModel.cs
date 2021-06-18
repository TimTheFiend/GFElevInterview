using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFElevInterview.Models
{
    public class ElevModel
    {
        [Key]
        public int CprNr { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }

        public string UddannelsesLinje { get; set; }
        public string UddannelsesAdresse { get; set; }
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
            return $"({CprNr} - {EfternavnFornavn}";
        }
    }
}
