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
                string _dag = CprNr.ToString().Substring(0, 2);
                string _måned = CprNr.ToString().Substring(2, 2);
                string _år = CprNr.ToString().Substring(4, 2);

                int dag = Int32.Parse(_dag);
                int måned = Int32.Parse(_måned);
                int år = Int32.Parse(_år);

                //010179
                if (år < DateTime.Now.Year - 1900 - 25 && år > DateTime.Now.Year - 2000) {
                    
                    return true;
                }

                år += 1900;
                if (!(år >= DateTime.Now.Year - 25))
                {
                    år += 100;
                }

                if (new DateTime(år, måned, dag) <= DateTime.Now.AddYears(-25))
                {
                    return true;
                }
                return false;
            }
        }
    }
}