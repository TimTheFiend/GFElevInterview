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

        private string meritFileSti = "Blanketter\\Templates\\Merit-blanket.docx";
        private string udprintSti = "..\\Blanketter";

        private string _nyMeritFile = $"{CurrentElev.elev.fornavn}.pdf";

        private string nyMeritFile = @"C:\Users\afba\Downloads\[TEST]\";

        //FreeSpire.Doc Version//
        Document doc = new Document();

        public bool udprintTilMerit() {
            try {
                //Henter "Template" fil fra given string sti.
                doc.LoadFromFile(meritFileSti);
                //Udksifter det valgt ord fra pdf´en med en ny værdi (Fra CurrentElev)
                doc.Replace("#navn#", CurrentElev.elev.efternavnFornavn, true, true);
                doc.Replace("#cpr#", CurrentElev.elev.cprNr, true, true);
                //doc.Replace("#navn#", CurrentElev.elev.FornavnEfternavn, true,true);
                //doc.Replace("#cpr#", CurrentElev.elev.CprNr.ToString(), true, true);
                doc.Replace("#DE#", CurrentElev.elev.danskPrintEksammen, true, true);
                doc.Replace("#DU#", CurrentElev.elev.danskPrintUndervisning, true, true);
                doc.Replace("#DN#", CurrentElev.elev.danskPrintNiveau, true, true);
                doc.Replace("#EE#", CurrentElev.elev.engelskPrintEksammen, true, true);
                doc.Replace("#EU#", CurrentElev.elev.engelskPrintUndervisning, true, true);
                doc.Replace("#EN#", CurrentElev.elev.engelskPrintNiveau, true, true);
                doc.Replace("#ME#", CurrentElev.elev.matematikPrintEksammen, true, true);
                doc.Replace("#MU#", CurrentElev.elev.matematikPrintUndervisning, true, true);
                doc.Replace("#MN#", CurrentElev.elev.matematikPrintNiveau, true, true);

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
