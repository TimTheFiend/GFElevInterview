using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;

namespace GFElevInterview.Data
{
    public class BlanketUdskrivning
    {
        private const string outputDirectory = @"..\Blanketter";
        DateTime now = DateTime.Now;
        public bool UdskrivningRKV()
        {
            string inputFil = GetFile();
            //TODO
            string outputFilePath = Path.Combine(outputDirectory, CurrentElev.elev.Fornavn + ".pdf");


            PdfReader pdfReader = new PdfReader(inputFil);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputFilePath, FileMode.Create));
            var pdfFormFields = pdfStamper.AcroFields;
            //Side 1 Elev Info
            pdfFormFields.SetField("Navn", CurrentElev.elev.FornavnEfternavn);
            pdfFormFields.SetField("Cprnr", CurrentElev.elev.CprNr);
            pdfFormFields.SetField("RKV gennemført", now.Day + "/" + now.Month);

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

            pdfStamper.Close();
            return false;
        }

        private string GetFile()
        {
            //TODO GetFile
            //string foo = $"EUV1 - {CurrentElev.elev.Uddannelse.ToString()}";
            //string foo = $"{CurrentElev.elev.IsRKV.ToString()} - {CurrentElev.elev.Uddannelse.ToString()}";

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string foo = "EUV1 - Infrastruktur.pdf";
            return "Blanketter\\Templates\\" + foo;
        }
    }
}