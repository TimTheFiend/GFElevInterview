using GFElevInterview.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Spire.Doc;
using System;
using System.Collections.Generic;
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
            try {
                string inputFil = GetRKVBlanketTemplate;
                //Udskrivnings filen.
                string outputFilePath = Path.Combine(outputDirectory, CurrentElev.elev.FilnavnRKV);
                PdfReader pdfReader = new PdfReader(inputFil);
                //filen skabes i outputFilePath slut lokationen.
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputFilePath, FileMode.Create));
                var pdfFormFields = pdfStamper.AcroFields;

                #region Indsætning af data

                //Side 1 Elev Info
                //Information sættes på feltes lokation ud fra deres navn
                pdfFormFields.SetField("Navn", CurrentElev.elev.FornavnEfternavn);
                pdfFormFields.SetField("Cprnr", CurrentElev.elev.CPRNr);
                pdfFormFields.SetField("RKV gennemført", DateTime.Now.Day + "/" + DateTime.Now.Month);

                //Side 3 Dansk
                pdfFormFields.SetField("Bemærk 1", CurrentElev.elev.DanNiveau.ToString());
                pdfFormFields.SetField("kompetence 1", "Dansk");
                pdfFormFields.SetField("Metrit 1", CurrentElev.elev.DanNiveau > FagNiveau.F ? "Yes" : "No");

                //Side 3 Matematik
                pdfFormFields.SetField("Bemærk 2", CurrentElev.elev.MatNiveau.ToString());
                pdfFormFields.SetField("kompetence 2", "Matematik");
                pdfFormFields.SetField("Metrit 2", CurrentElev.elev.MatNiveau > FagNiveau.F ? "Yes" : "No");

                //Side 3 Engelsk
                pdfFormFields.SetField("Bemærk 3", CurrentElev.elev.EngNiveau.ToString());
                pdfFormFields.SetField("kompetence 3", "Engelsk");
                pdfFormFields.SetField("Metrit 3", CurrentElev.elev.EngNiveau > FagNiveau.F ? "Yes" : "No");

                //NOTE laver udregningen her, ellers er der ingen værdi.
                CurrentElev.elev.BeregnMeritIUger();

                pdfFormFields.SetField("GF antal dage 1", "100");
                pdfFormFields.SetField("GF Samlet antal dages merit", $"{ CurrentElev.elev.MeritLængdeIDage}");
                pdfFormFields.SetField("GF Samlet antal på GF", $"{100 - CurrentElev.elev.MeritLængdeIDage}");

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
                if (CurrentElev.elev.ErRKV) {
                    UdskrivningRKV();
                }

                Spire.Doc.Document doc = new Spire.Doc.Document();
                //Henter "Template" fil fra given string sti.
                doc.LoadFromFile(GetMeritBlanketTemplate);

                #region Udskiftning af værdier

                doc.Replace("#navn#", CurrentElev.elev.EfternavnFornavn, true, true);
                doc.Replace("#cpr#", CurrentElev.elev.CPRNr, true, true);

                //"Oversætter" Flag-værdier til `string` værdier
                ElevModel elev = CurrentElev.elev;
                doc.Replace("#DE#", GetStringFromFlag(elev, Merit.DanskEksamen), true, true);
                doc.Replace("#DU#", GetStringFromFlag(elev, Merit.DanskUndervisning), true, true);
                doc.Replace("#EE#", GetStringFromFlag(elev, Merit.EngelskEksamen), true, true);
                doc.Replace("#EU#", GetStringFromFlag(elev, Merit.EngelskUndervisning), true, true);
                doc.Replace("#ME#", GetStringFromFlag(elev, Merit.MatematikEksamen), true, true);
                doc.Replace("#MU#", GetStringFromFlag(elev, Merit.MatematikUndervisning), true, true);

                doc.Replace("#DN#", CurrentElev.elev.DanNiveau.ToString(), true, true);
                doc.Replace("#EN#", CurrentElev.elev.EngNiveau.ToString(), true, true);
                doc.Replace("#MN#", CurrentElev.elev.MatNiveau.ToString(), true, true);
                doc.Replace("#uger#", CurrentElev.elev.UddLængdeIUger.ToString(), true, true);

                #endregion Udskiftning af værdier

                //throw new Exception();
                doc.SaveToFile(Path.Combine(outputDirectory, CurrentElev.elev.FilnavnMerit), FileFormat.PDF);
                return true;
            }
            catch (Exception) {
                return false;
                throw;
            }
        }

        /// <summary>
        /// Jeg vil få til sendt en liste og en string fra vincent del.
        /// Metoden skal tage imod disse data´er og putte dem ud i linjer efter hver elev.
        /// 
        /// 1. Sæt informationer givet i parameterne, ind i et excel ark.
        /// . Et loop skal tage data´en med hver elev 
        /// . Lav et loop som sætter ind alle eleverne og deres informationer, en linje ad gangen.
        /// . (Informationerne sættes ind i et excel ark som bliver konverteret videre til et pdf format).
        /// 2. Initialiser Konverteringen af pdf processen.
        /// 3. Udskriv pdf dokumentet til blanketten.
        /// </summary>
        public static PdfPTable UdskrivningDataTabel(List<ElevModel> elever, string query, PdfPTable tableLayout)
        {
            //PdfPTable tableLayout;
            query = "Elever der vil have SPS";
            //Floaten giver 
            float[] headers =
            {
                20, 20, 20, 20
            };
            //Headernes bredde vil blive sat her.
            tableLayout.SetWidths(headers);
            tableLayout.WidthPercentage = 80;
            tableLayout.AddCell(new PdfPCell(new Phrase("Den valgte "+query))
            {
                Colspan = 4,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            AddCellToHeader(tableLayout, "CPR");
            AddCellToHeader(tableLayout, "Fornavn");
            AddCellToHeader(tableLayout, "Efternavn");
            AddCellToHeader(tableLayout, "Valgte");
            foreach (var item in elever)
            {
                AddCellToBody(tableLayout, item.CPRNr);
                AddCellToBody(tableLayout, item.Fornavn);
                AddCellToBody(tableLayout, item.Efternavn);
                AddCellToBody(tableLayout, query);
            }
            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
            });
        }
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
            });
        }

        /// <summary>
        /// Returnerer "Ja"/"Nej" fra en <see cref="bool"/> værdi.
        /// </summary>
        /// <param name="elev">Den nuværende elev</param>
        /// <param name="flag">Det pågældende fag</param>
        /// <returns></returns>
        private static string GetStringFromFlag(ElevModel elev, Merit flag) {
            return elev.UddMerit.HasFlag(flag) ? "Ja" : "Nej";
        }

        /// <summary>
        /// Laver et filnavn til brug for RKV-blanket.
        /// </summary>
        /// <returns>RKV filnavn.</returns>
        private static string GetRKVBlanketTemplate {
            get {
                string pdfElev = $"{CurrentElev.elev.ElevType.ToString()} - {CurrentElev.elev.UddLinje}.pdf";
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