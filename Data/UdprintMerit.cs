using Spire.Doc;

namespace GFElevInterview.Data
{
    //TODO: Lav metode som returnere en string for filnavn samt lokation.
    public class UdprintMerit
    {
        /*                                  SUMMARY:
         * I denne klasse udprintes informationer fra skabelonerne til pdf´erne.
         * Alt information vil blive hentet fra en template dokument og overført til et nyt dokument.
         * TO DO:
         * Der skal findes en måde at køre replace bedre på.
         * Tilføj Merit og RKV til udprintning.
         */

        private string meritSkabelon = "Blanketter\\Templates\\Meritblanket.docx";
        private string saveFolder = "..\\Blanketter\\Udfyldte blanketter";
        private string nyMeritFile = @"C:\Users\joak\Downloads\[TEST]\";


        //FreeSpire.Doc Version//
        Document doc = new Document();
        public void UdprintTilMerit() {
            //For at gøre det mere overskueligt
            Fag dansk = CurrentElev.meritBlanket.Dansk;
            Fag engelsk = CurrentElev.meritBlanket.Engelsk;
            Fag matematik = CurrentElev.meritBlanket.Matematik;


            //Henter "Template" fil fra given string sti.
            doc.LoadFromFile(meritSkabelon);
            //Udksifter det valgt ord fra pdf´en med en ny værdi (Fra CurrentElev)
            doc.Replace("#navn#", CurrentElev.elev.EfternavnFornavn, true, true);
            doc.Replace("#cpr#", CurrentElev.elev.CprNr.ToString(), true, true);
            //doc.Replace("#navn#", CurrentElev.elev.FornavnEfternavn, true,true);
            //doc.Replace("#cpr#", CurrentElev.elev.CprNr.ToString(), true, true);
            doc.Replace("#DE#", dansk.udprintEksamen, true, true);
            doc.Replace("#DU#", dansk.udprintUndervisning, true, true);
            doc.Replace("#DN#", dansk.udprintNiveau, true, true);
            doc.Replace("#EE#", engelsk.udprintEksamen, true, true);
            doc.Replace("#EU#", engelsk.udprintUndervisning, true, true);
            doc.Replace("#EN#", engelsk.udprintNiveau, true, true);
            doc.Replace("#ME#", matematik.udprintEksamen, true, true);
            doc.Replace("#MU#", matematik.udprintUndervisning, true, true);
            doc.Replace("#MN#", matematik.udprintNiveau, true, true);
            doc.SaveToFile(GetSavePath(), FileFormat.PDF);
            //doc.SaveToFile(nyMeritFile + CurrentElev.elev.EfternavnFornavn + ".pdf", FileFormat.PDF);
        }

        private string GetSavePath() {
            if (!System.IO.Directory.Exists(saveFolder)) {
                System.IO.Directory.CreateDirectory(saveFolder);
            }

            return System.IO.Path.Combine(saveFolder, CurrentElev.elev.EfternavnFornavn + ".pdf");
        }
    }
}
