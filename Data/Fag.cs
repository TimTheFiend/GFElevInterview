namespace GFElevInterview.Data
{
    public class Fag
    {
        public bool? Eksamen { get; set; } = null;
        public bool? Undervisning { get; set; } = null;
        public FagNiveau Niveau { get; set; }

        public string udprintEksamen { get { return (bool)Eksamen ? "Ja" : "Nej"; } }
        public string udprintUndervisning { get { return (bool)Undervisning ? "Ja" : "Nej"; } }

        public string udprintNiveau { get { return Niveau.ToString(); } }

        public Fag() {
        }

        public Fag(bool eksamen, bool undervisning, FagNiveau niveau) {
            Eksamen = eksamen;
            Undervisning = undervisning;
            Niveau = niveau;
        }

        public bool HasSetValues {
            get {
                if (Eksamen != null || Undervisning != null || Niveau != FagNiveau.Null) {
                    return true;
                }
                return false;
            }
        }
    }
}