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

        /// <summary>
        /// En stig til filens position laves og fil-objekt bliver oprettet.
        /// Informationen over eleven hentes.
        /// informationen indsættes i felterne, og gemmes som pdf fil.
        /// </summary>
        public void UdskrivningRKV()
        {
            try {
                string inputFil = GetRKVBlanketTemplate();
                //Udskrivnings filen.
                string outputFilePath = Path.Combine(outputDirectory, CurrentElev.elev.fornavn + ".pdf");
                PdfReader pdfReader = new PdfReader(inputFil);
                //filen skabes i outputFilePath slut lokationen.
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputFilePath, FileMode.Create));
                var pdfFormFields = pdfStamper.AcroFields;
                #region Indsætning af data
                //Side 1 Elev Info
                //Information sættes på feltes lokation ud fra deres navn
                pdfFormFields.SetField("Navn", CurrentElev.elev.FornavnEfternavn);
                pdfFormFields.SetField("Cprnr", CurrentElev.elev.cprNr);
                pdfFormFields.SetField("RKV gennemført", DateTime.Now.Day + "/" + DateTime.Now.Month);

                //Side 3 Dansk
                pdfFormFields.SetField("Bemærk 1", CurrentElev.elev.danskNiveau.ToString());
                pdfFormFields.SetField("kompetence 1", "Dansk");
                pdfFormFields.SetField("Metrit 1", "Yes");

                //Side 3 Matematik
                pdfFormFields.SetField("Bemærk 2", CurrentElev.elev.matematikNiveau.ToString());
                pdfFormFields.SetField("kompetence 2", "Matematik");
                pdfFormFields.SetField("Metrit 2", "Yes");

                //Side 3 Engelsk
                pdfFormFields.SetField("Bemærk 3", CurrentElev.elev.engelskNiveau.ToString());
                pdfFormFields.SetField("kompetence 3", "Engelsk");
                pdfFormFields.SetField("Metrit 3", "Yes");

                //TODO Side 4 Grundforløb
                pdfFormFields.SetField("GF antal dage 1", "100");
                pdfFormFields.SetField("GF Samlet antal dages merit", $"{ CurrentElev.elev.meritLængdeIDage * -1}");
                pdfFormFields.SetField("GF Samlet antal på GF", $"{100 + CurrentElev.elev.meritLængdeIDage}");

                //TODO MAX
                //Side 5 Special Støtte
                //pdfFormFields.SetField("Specialpæd ja", CurrentElev.elev.SPS.ToString());
                //pdfFormFields.SetField("GF Samlet antal dages merit", "10");
                //pdfFormFields.SetField("GF Samlet antal på GF", "25");
                #endregion

                //Gemmer og lukker for fil-objektet.
                pdfStamper.Close();
            }
            catch (Exception) {
                throw;
            }

        }

        /// <summary>
        /// Opretter et <see cref="Document"/> og loader det tilsvarende blanket skabelon ind
        /// baseret på <see cref="CurrentElev"/> elevtype og uddannelse, og gemmer filen.
        /// </summary>
        /// <returns><c>true</c> hvis udprintningen er succesfuld; ellers <c>false</c>.</returns>
        public bool UdskrivningMerit() {
            try {
                if (CurrentElev.elev.ErRKV) {
                    UdskrivningRKV();
                }

                Document doc = new Document();
                //Henter "Template" fil fra given string sti.
                doc.LoadFromFile(GetMeritBlanketTemplate);

                #region Udskiftning af værdier

                doc.Replace("#navn#", CurrentElev.elev.EfternavnFornavn, true, true);
                doc.Replace("#cpr#", CurrentElev.elev.cprNr, true, true);
                //doc.Replace("#navn#", CurrentElev.elev.FornavnEfternavn, true,true);
                //doc.Replace("#cpr#", CurrentElev.elev.CprNr.ToString(), true, true);
                doc.Replace("#DE#", CurrentElev.elev.danskEksammen ? "Ja" : "Nej", true, true);
                doc.Replace("#DU#", CurrentElev.elev.danskUndervisning ? "Ja" : "Nej", true, true);
                doc.Replace("#DN#", CurrentElev.elev.danskNiveau.ToString(), true, true);
                doc.Replace("#EE#", CurrentElev.elev.engelskEksammen ? "Ja" : "Nej", true, true);
                doc.Replace("#EU#", CurrentElev.elev.engelskUndervisning ? "Ja" : "Nej", true, true);
                doc.Replace("#EN#", CurrentElev.elev.engelskNiveau.ToString(), true, true);
                doc.Replace("#ME#", CurrentElev.elev.matematikEksammen ? "Ja" : "Nej", true, true);
                doc.Replace("#MU#", CurrentElev.elev.matematikUndervisning ? "Ja" : "Nej", true, true);
                doc.Replace("#MN#", CurrentElev.elev.matematikNiveau.ToString(), true, true);
                doc.Replace("#uger#", CurrentElev.elev.uddannelsesLængdeIUger.ToString(), true, true);
                
                #endregion
               
                doc.SaveToFile(Path.Combine(outputDirectory, CurrentElev.elev.FilNavn), FileFormat.PDF);
                return true;
            }
            catch (Exception) {
                return false;
                throw;
            }
        }

        private string GetRKVBlanketTemplate()
        {
            string pdfElev = $"{CurrentElev.elev.elevType.ToString()} - {CurrentElev.elev.uddannelse}.pdf";
            return Path.Combine(config.AppSettings["templates"], pdfElev);
        }

        private string GetMeritBlanketTemplate {
            get {
                return Path.Combine(config.AppSettings["templates"], "Merit-blanket.docx");
            }
        }
    }
}