using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GFElevInterview.Models
{
    public class Elev
    {
        [Key]
        public int CprNr { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
    }
}
