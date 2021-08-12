using GFElevInterview.Data;

namespace GFElevInterview.Models
{
    /// <summary>
    /// Dataholder til udskriving af relevante blanketter vedr. fag.
    /// </summary>
    public class FagModel
    {
        public bool Eksamen { get; set; }
        public bool Undervisning { get; set; }
        public FagNiveau Niveau { get; set; }

        /// Properties der returnerer en string der bruges til udprint.
        public string udprintEksammen { get { return Eksamen ? "Ja" : "Nej"; } }

        public string udprintUndervisning { get { return Undervisning ? "Ja" : "Nej"; } }

        public string udprintNiveau { get { return Niveau.ToString(); } }

        public FagModel() {
        }

        public FagModel(bool eksamen, bool undervisning, FagNiveau niveau) {
            Eksamen = eksamen;
            Undervisning = undervisning;
            Niveau = niveau;
        }
    }
}