
namespace GFElevInterview.Data
{
    public class Fag
    {
        public bool Eksamen { get; set; }
        public bool Undervisning { get; set; }
        public FagNiveau Niveau { get; set; }

        public Fag(bool eksamen, bool undervisning, FagNiveau niveau)
        {
            Eksamen = eksamen;
            Undervisning = undervisning;
            Niveau = niveau;
        }
    }
}
