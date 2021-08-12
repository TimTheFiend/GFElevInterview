using Spire.Doc;

namespace GFElevInterview.Data
{
    /// <summary>
    /// I denne klasse udprintes informationer fra skabelonerne til pdf filer.
    /// Udvalgte ord fra skabelonerne vil blive udskiftet med brugerens input og gemt i en pdf fil.
    /// Ting som Word delen af interviewet vil blive gemt i databasen.
    /// </summary>
    public class UdprintMerit
    {

        private string meritFileSti = "Blanketter\\Templates\\Meritblanketter VISI blank.docx";
        private string udprintSti = "..\\Blanketter";

        private string _nyMeritFile = $"{CurrentElev.elev.Fornavn}.pdf";

        private string nyMeritFile = @"C:\Users\viga\Downloads\[TEST]\";

        //FreeSpire.Doc Version//
        Document doc = new Document();

        public bool udprintTilMerit() {
            try {
                //Henter "Template" fil fra given string sti.
                doc.LoadFromFile(meritFileSti);
                //Udksifter det valgt ord fra pdf´en med en ny værdi (Fra CurrentElev)
                doc.Replace("#navn#", CurrentElev.elev.EfternavnFornavn, true, true);
                doc.Replace("#cpr#", CurrentElev.elev.CprNr, true, true);
                //doc.Replace("#navn#", CurrentElev.elev.FornavnEfternavn, true,true);
                //doc.Replace("#cpr#", CurrentElev.elev.CprNr.ToString(), true, true);
                doc.Replace("#DE#", CurrentElev.meritBlanket.Dansk.udprintEksammen, true, true);
                doc.Replace("#DU#", CurrentElev.meritBlanket.Dansk.udprintUndervisning, true, true);
                doc.Replace("#DN#", CurrentElev.meritBlanket.Dansk.udprintNiveau, true, true);
                doc.Replace("#EE#", CurrentElev.meritBlanket.Engelsk.udprintEksammen, true, true);
                doc.Replace("#EU#", CurrentElev.meritBlanket.Engelsk.udprintUndervisning, true, true);
                doc.Replace("#EN#", CurrentElev.meritBlanket.Engelsk.udprintNiveau, true, true);
                doc.Replace("#ME#", CurrentElev.meritBlanket.Matematik.udprintEksammen, true, true);
                doc.Replace("#MU#", CurrentElev.meritBlanket.Matematik.udprintUndervisning, true, true);
                doc.Replace("#MN#", CurrentElev.meritBlanket.Matematik.udprintNiveau, true, true);

                doc.SaveToFile(System.IO.Path.Combine(udprintSti, CurrentElev.elev.FilNavn), FileFormat.PDF);
                return true;
            }
            catch (System.Exception) {
                // TODO
                return false;
            }

        }
    }
}
