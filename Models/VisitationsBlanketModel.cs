using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Models
{
    //NOTE: Skal ikke bruges
    public class VisitationsBlanketModel
    {
        public string Uddannelse { get; set; }
        public string UdannelseAdresse { get; set; }

        public bool SPS { get; set; }
        public bool EUD { get; set; }
        //public string SPSVisitations { get { return SPS ? "Ja" : "Nej"; } set { } }
        //public string EUDVisitations { get { return EUD ? "Ja" : "Nej"; } set { } }
    }
}
