using GFElevInterview.Models;
using iTextSharp.text.pdf;
using Spire.Doc;
using System;
using System.IO;

namespace GFElevInterview.Data
{
    public static class BlanketUdskrivning
    {
        private static readonly string outputDirectory = RessourceFil.outputMappe;

        //Static constructor
        static BlanketUdskrivning() {
            if (!Directory.Exists(outputDirectory)) {
                Directory.CreateDirectory(outputDirectory);
            }
        }

        /// <summary>
        /// En stig til filens position laves og fil-objekt bliver oprettet.
        /// Informationen over eleven hentes.
        /// informationen indsættes i felterne, og gemmes som pdf fil.
        /// </summary>
        public static void UdskrivningRKV() {
            //TODO `Metrit #` skal ændres til at matche med minimums karakter
            try {
                string inputFil = GetRKVBlanketTemplate;
                //Udskrivnings filen.
                string outputFilePath = Path.Combine(outputDirectory, CurrentElev.elev.RKVFilNavn);
                PdfReader pdfReader = new PdfReader(inputFil);
                //filen skabes i outputFilePath slut lokationen.
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputFilePath, FileMode.Create));
                var pdfFormFields = pdfStamper.AcroFields;

                #region Indsætning af data

                //Side 1 Elev Info
                //Information sættes på feltes lokation ud fra deres navn
                pdfFormFields.SetField("Navn", CurrentElev.elev.fornavnEfternavn);
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
                //NOTE laver udregningen her, ellers er der ingen værdi.
                CurrentElev.elev.BeregnMeritIUger();

                pdfFormFields.SetField("GF antal dage 1", "100");
                pdfFormFields.SetField("GF Samlet antal dages merit", $"{ CurrentElev.elev.meritLængdeIDage}");
                pdfFormFields.SetField("GF Samlet antal på GF", $"{100 - CurrentElev.elev.meritLængdeIDage}");

                //TODO MAX
                //Side 5 Special Støtte
                //pdfFormFields.SetField("Specialpæd ja", CurrentElev.elev.SPS.ToString());
                //pdfFormFields.SetField("GF Samlet antal dages merit", "10");
                //pdfFormFields.SetField("GF Samlet antal på GF", "25");

                #endregion Indsætning af data

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
        public static bool UdskrivningMerit() {
            try {
                if (CurrentElev.elev.erRKV) {
                    UdskrivningRKV();
                }

                Document doc = new Document();
                //Henter "Template" fil fra given string sti.
                doc.LoadFromFile(GetMeritBlanketTemplate);

                #region Udskiftning af værdier

                doc.Replace("#navn#", CurrentElev.elev.efternavnFornavn, true, true);
                doc.Replace("#cpr#", CurrentElev.elev.cprNr, true, true);

                //"Oversætter" Flag-værdier til `string` værdier
                ElevModel elev = CurrentElev.elev;
                doc.Replace("#DE#", GetStringFromFlag(elev, Merit.DanskEksamen), true, true);
                doc.Replace("#DU#", GetStringFromFlag(elev, Merit.DanskUndervisning), true, true);
                doc.Replace("#EE#", GetStringFromFlag(elev, Merit.EngelskEksamen), true, true);
                doc.Replace("#EU#", GetStringFromFlag(elev, Merit.EngelskUndervisning), true, true);
                doc.Replace("#ME#", GetStringFromFlag(elev, Merit.MatematikEksamen), true, true);
                doc.Replace("#MU#", GetStringFromFlag(elev, Merit.MatematikUndervisning), true, true);

                doc.Replace("#DN#", CurrentElev.elev.danskNiveau.ToString(), true, true);
                doc.Replace("#EN#", CurrentElev.elev.engelskNiveau.ToString(), true, true);
                doc.Replace("#MN#", CurrentElev.elev.matematikNiveau.ToString(), true, true);
                doc.Replace("#uger#", CurrentElev.elev.uddannelsesLængdeIUger.ToString(), true, true);

                #endregion Udskiftning af værdier

                //throw new Exception();
                doc.SaveToFile(Path.Combine(outputDirectory, CurrentElev.elev.MeritFilNavn), FileFormat.PDF);
                return true;
            }
            catch (Exception) {
                return false;
                throw;
            }
        }

        /// <summary>
        /// Returnerer "Ja"/"Nej" fra en <see cref="bool"/> værdi.
        /// </summary>
        /// <param name="elev">Den nuværende elev</param>
        /// <param name="flag">Det pågældende fag</param>
        /// <returns></returns>
        private static string GetStringFromFlag(ElevModel elev, Merit flag) {
            return elev.uddannelsesMerit.HasFlag(flag) ? "Ja" : "Nej";
        }

        /// <summary>
        /// Laver et filnavn til brug for RKV-blanket.
        /// </summary>
        /// <returns>RKV filnavn.</returns>
        private static string GetRKVBlanketTemplate {
            get {
                string pdfElev = $"{CurrentElev.elev.elevType.ToString()} - {CurrentElev.elev.uddannelse}.pdf";
                return Path.Combine(RessourceFil.templates, pdfElev);
            }
        }

        /// <summary>
        /// Laver et filnavn til brug for Merit-blanket.
        /// </summary>
        /// <returns>Merit filnavn.</returns>
        private static string GetMeritBlanketTemplate {
            get {
                return Path.Combine(RessourceFil.templates, RessourceFil.meritBlanketDoc);
            }
        }
    }
}