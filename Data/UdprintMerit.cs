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
        private string fileSti = "C:\\Users\\viga\\Desktop\\Meritblanketter VISI blank.docx";
        private string nyFile = "C:\\Users\\viga\\Desktop\\Test Merit.pdf";

        public void udprintFraWord()
        {
            //FreeSpire.Doc Version//
            Document doc = new Document();
            //Henter "Template" fil fra given string sti.
            doc.LoadFromFile(fileSti);
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
            doc.SaveToFile(nyFile, Spire.Doc.FileFormat.PDF);
        }
    }
}
