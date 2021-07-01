using System;
using System.IO;
using Spire.Doc;
using iTextSharp.text.pdf;
using config = System.Configuration.ConfigurationManager;

namespace GFElevInterview.Data
{
    public class BlanketUdskrivning
    {
        private readonly string outputDirectory = config.AppSettings["outputMappe"];

        public BlanketUdskrivning() {
            if (!Directory.Exists(outputDirectory)) {
                Directory.CreateDirectory(outputDirectory);
            }
        }

        public bool UdskrivningRKV()
        {
            try {
                string inputFil = GetRKVTemplate();
                //Sæt hvor filen skal gemmes henne.
                string outputFilePath = Path.Combine(outputDirectory, CurrentElev.elev.Fornavn + ".pdf");

                PdfReader pdfReader = new PdfReader(inputFil);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputFilePath, FileMode.Create));
                var pdfFormFields = pdfStamper.AcroFields;
                #region Indsætning af data
                //Side 1 Elev Info
                pdfFormFields.SetField("Navn", CurrentElev.elev.FornavnEfternavn);
                pdfFormFields.SetField("Cprnr", CurrentElev.elev.CprNr);
                pdfFormFields.SetField("RKV gennemført", DateTime.Now.Day + "/" + DateTime.Now.Month);

                //Side 3 Dansk
                pdfFormFields.SetField("Bemærk 1", CurrentElev.meritBlanket.Dansk.Niveau.ToString());
                pdfFormFields.SetField("kompetence 1", "Dansk");
                pdfFormFields.SetField("Metrit 1", "Yes");

                //Side 3 Matematik
                pdfFormFields.SetField("Bemærk 2", CurrentElev.meritBlanket.Matematik.Niveau.ToString());
                pdfFormFields.SetField("kompetence 2", "Matematik");
                pdfFormFields.SetField("Metrit 2", "Yes");

                //Side 3 Engelsk
                pdfFormFields.SetField("Bemærk 3", CurrentElev.meritBlanket.Engelsk.Niveau.ToString());
                pdfFormFields.SetField("kompetence 3", "Engelsk");
                pdfFormFields.SetField("Metrit 3", "Yes");

                //TODO Side 4 Grundforløb
                pdfFormFields.SetField("GF antal dage 1", "100");
                pdfFormFields.SetField("GF Samlet antal dages merit", $"{ CurrentElev.meritBlanket.MeritLængdeIDage * -1}");
                pdfFormFields.SetField("GF Samlet antal på GF", $"{100 + CurrentElev.meritBlanket.MeritLængdeIDage}");

                //TODO MAX
                //Side 5 Special Støtte
                //pdfFormFields.SetField("Specialpæd ja", CurrentElev.elev.SPS.ToString());
                //pdfFormFields.SetField("GF Samlet antal dages merit", "10");
                //pdfFormFields.SetField("GF Samlet antal på GF", "25");
                #endregion
                pdfStamper.Close();
                return true;
            }
            catch (Exception) {
                return false;
                throw;
            }

        }

        public bool UdskrivningMerit() {
            try {
                if (CurrentElev.elev.IsRKV) {
                    UdskrivningRKV();
                }

                Document doc = new Document();
                //Henter "Template" fil fra given string sti.
                doc.LoadFromFile(GetMeritTemplate);
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
                doc.Replace("#uger#", CurrentElev.meritBlanket.UddannelsesLængdeIUger.ToString(), true, true);

                doc.SaveToFile(Path.Combine(outputDirectory, CurrentElev.elev.FilNavn), FileFormat.PDF);
                //doc.SaveToFile(nyMeritFile + _nyMeritFile, FileFormat.PDF);

                return true;
            }
            catch (Exception) {
                return false;
                throw;
            }
        }

        private string GetRKVTemplate()
        {
            string foo = $"{CurrentElev.elev.ElevType.ToString()} - {CurrentElev.elev.Uddannelse}.pdf";
            return Path.Combine(config.AppSettings["templates"], foo);
        }

        private string GetMeritTemplate {
            get {
                return Path.Combine(config.AppSettings["templates"], "Merit-blanket.docx");
            }
        }
    }
}