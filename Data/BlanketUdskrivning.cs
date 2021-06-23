using System;
using System.IO;
using Spire.Doc;
using iTextSharp.text.pdf;

namespace GFElevInterview.Data
{
    public class BlanketUdskrivning
    {
        private const string outputDirectory = @"..\Blanketter";
        
        public BlanketUdskrivning() {
            if (!Directory.Exists(outputDirectory)) {
                Directory.CreateDirectory(outputDirectory);
            }
        }

        public bool UdskrivningRKV()
        {
            try {
                string inputFil = GetRKVTemplate();
                //TODO
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
                pdfFormFields.SetField("GF antal dage 1", "00");
                pdfFormFields.SetField("GF Samlet antal dages merit", "10");
                pdfFormFields.SetField("GF Samlet antal på GF", "25");

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

                throw;
            }

            return false;
        }

        public bool UdskrivningMerit() {
            try {
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

                doc.SaveToFile(System.IO.Path.Combine(outputDirectory, CurrentElev.elev.FilNavn), FileFormat.PDF);
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
            string foo = "EUV1 - Infrastruktur.pdf";
            return "Blanketter\\Templates\\" + foo;
        }

        private string GetMeritTemplate {
            get {
                return @"Blanketter\Templates\Merit-blanket.docx";
            }
        }
    }
}