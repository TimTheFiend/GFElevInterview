using GFElevInterview.Data;

namespace GFElevInterview.Models
{
    /// <summary>
    /// Dataholder til udskriving af relevante blanketter vedr. fag.
    /// </summary>
    //DEPRECATED
    public class FagModel
    {
        //samme navn som i elev model
        public bool Eksamen { get; set; }
        public bool Undervisning { get; set; }
        public FagNiveau Niveau { get; set; }

        public FagModel() {
        }

        public FagModel(bool eksamen, bool undervisning, FagNiveau niveau) {
            Eksamen = eksamen;
            Undervisning = undervisning;
            Niveau = niveau;
        }
    }
}