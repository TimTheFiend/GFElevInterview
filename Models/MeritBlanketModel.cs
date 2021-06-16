using System;
using System.Collections.Generic;
using System.Text;
using GFElevInterview.Data;

namespace GFElevInterview.Models
{
    public class MeritBlanketModel
    {
        public Fag Dansk;
        public Fag Engelsk;
        public Fag Matematik;

        public MeritBlanketModel()
        {
        }

        public MeritBlanketModel(Fag dansk)
        {
            Dansk = dansk;
        }

        public MeritBlanketModel(Fag dansk, Fag engelsk, Fag matematik)
        {
            Dansk = dansk;
            Engelsk = engelsk;
            Matematik = matematik;
        }
    }
}
