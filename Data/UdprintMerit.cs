using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Data
{
    public class UdprintMerit
    {
        /*                                  SUMMARY:
         * I denne klasse udprintes informationer fra skabelonerne til pdf´erne.
         * Alt information vil blive hentet fra en template dokument og overført til et nyt dokument.
         * TO DO:
         * Der skal findes en måde at køre replace bedre på.
         * Tilføj Merit og RKV til udprintning.
         */
        private string meritFileSti = "C:\\Users\\viga\\Desktop\\Meritblanketter VISI blank.docx";
        private string nyMeritFile = "C:\\Users\\viga\\Desktop\\Test Merit.pdf";
        private string wordFileSti = "C:\\Users\\viga\\Desktop\\Wordblanketter.docx";
        private string nyWordFile = "C:\\Users\\viga\\Desktop\\Test Word Interview.pdf";
        //FreeSpire.Doc Version//
        Document doc = new Document();
        public void udprintTilMerit()
        {           
            //Henter "Template" fil fra given string sti.
            doc.LoadFromFile(meritFileSti);
            //Udksifter det valgt ord fra pdf´en med en ny værdi (Fra CurrentElev)
            doc.Replace("#navn#", "Mark Thomsen", true, true);
            doc.Replace("#cpr#", "12345", true, true);
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
            doc.SaveToFile(nyMeritFile, Spire.Doc.FileFormat.PDF);
        }
        public void udprintTilWord()
        {
            //Henter "Template" fil fra given string sti.
            doc.LoadFromFile(wordFileSti);
            //Udksifter det valgt ord fra pdf´en med en ny værdi (Fra CurrentElev)
            doc.Replace("#navn#", "Mark Thomsen", true, true);
            doc.Replace("#cpr#", "12345", true, true);
            //doc.Replace("#navn#", CurrentElev.elev.FornavnEfternavn, true,true);
            //doc.Replace("#cpr#", CurrentElev.elev.CprNr.ToString(), true, true);
            doc.Replace("#Uddannelse#", CurrentElev.visitationsBlanket.Uddannelse, true, true);
            doc.Replace("#UddannelseAddresse#", CurrentElev.visitationsBlanket.UdannelseAdresse, true, true);
            doc.Replace("#Sps#", CurrentElev.visitationsBlanket.SPSVisitations, true, true);
            doc.Replace("#Eud#", CurrentElev.visitationsBlanket.EUDVisitations, true, true);
            doc.SaveToFile(nyWordFile, Spire.Doc.FileFormat.PDF);
        }
    }
}
