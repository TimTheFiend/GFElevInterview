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
        public int CprNr { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }

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

        public string FullInfo
        {
            get { return this.ToString(); }
            
        }
    }
}
