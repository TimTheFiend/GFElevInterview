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
            Dansk = new Fag();
            Engelsk = new Fag();
            Matematik = new Fag();
        }

        public bool IsFilled
        {
            get
            {
                if (Dansk.Niveau != FagNiveau.Null && Engelsk.Niveau != FagNiveau.Null && Matematik.Niveau != FagNiveau.Null)
                {
                    return true;
                }
                return false;
            }
        }

        public List<string> AvailableSchools() {
            if (Dansk.Niveau <= FagNiveau.F) {
                return new List<string>() {
                    "Ballerup"
                };
            }
            return new List<string>() {
                "Frederiksberg",
                "Lyngby"
            };
        }

        //TODO: Proper check
        public List<string> AvailableEducations() {
            return new List<string>();
        }
    }
}
