using GFElevInterview.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace GFElevInterview.Tools
{
    public static class FilHandler
    {
        public static bool SletDokumenterIOutputMappe() {
            try {
                foreach (string filEndelse in new string[] {
                RessourceFil.endMerit,
                RessourceFil.endRKV}) {
                    foreach (string fil in HentFiler(filEndelse)) {
                        File.Delete(fil);
                    }
                }
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Henter en liste af relevante filer i <see cref="RessourceFil.outputMappeNavn"/> mappen.
        /// </summary>
        /// <param name="endelse">Endelsen på filerne der skal hentes.</param>
        /// <returns><see cref="List"/> af filers filsti.</returns>
        public static List<string> HentFiler(string endelse) {
            List<string> filer = new List<string>();
            try {
                filer = Directory.GetFiles(RessourceFil.outputMappe, $"*{endelse}").ToList();
            }
            catch (DirectoryNotFoundException) {
                //Just in case.
            }
            return filer;
        }

        /// <summary>
        /// Kombinerer alle merit-blanketter ind i én .PDF-fil.
        /// </summary>
        public static bool KombinerMeritFiler() {
            List<string> filNavne = HentFiler(RessourceFil.endMerit);
            if (filNavne.Count == 0) {
                return false;
            }

            Document nytDokument = new Document();
            using (FileStream fs = new FileStream(RessourceFil.samletMerit, FileMode.Create)) {
                PdfCopy writer = new PdfCopy(nytDokument, fs);
                if (writer == null) {
                    return false;
                }

                nytDokument.Open();

                foreach (string filNavn in filNavne) {
                    PdfReader reader = new PdfReader(filNavn);

                    for (int i = 1; i <= reader.NumberOfPages; i++) {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }
                    reader.Close();
                }
                //NOTE Hvis der ikke er nogle sider, så crasher.
                writer.Close();
                nytDokument.Close();
                return true;
            }
        }

        /// <summary>
        /// Zipper alle RKV-blanketter ind i én zip-fil.
        /// </summary>
        public static bool ZipRKVFiler() {
            List<string> filNavne = HentFiler(RessourceFil.endRKV);
            if (filNavne.Count == 0) {
                return false;
            }

            string filSti = RessourceFil.samletRKV;
            if (File.Exists(filSti)) {
                File.Delete(filSti);
            }

            var zip = ZipFile.Open(filSti, ZipArchiveMode.Create);
            foreach (string filNavn in filNavne) {
                zip.CreateEntryFromFile(filNavn, Path.GetFileName(filNavn), CompressionLevel.Optimal);
            }
            zip.Dispose();
            return true;
        }

        //TODO Joakim kommentar.
        public static void VisFilIExplorer(bool erICurdir, params string[] objNavne)
        {
            string curDir = Directory.GetCurrentDirectory();
            List<string> filSti = objNavne.ToList();

            if (!erICurdir) {
                filSti.Insert(0, RessourceFil.outputMappeNavn);
                curDir = curDir.Substring(0, curDir.LastIndexOf('\\'));
            }
            filSti.Insert(0, curDir);
            Process.Start("explorer.exe", $"/select,\"{ Path.Combine(filSti.ToArray()) }");  //"/select," highlighter den valgte fil.
        }

        /// <summary>
        /// Åbner et <see cref="OpenFileDialog"/> vindue, til tilføjelse af Excel-ark til databasen.
        /// </summary>
        public static void OpenFileDialog() {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            openFile.Filter = "Excel File |*.xls;*.xlsx;*.xlsm";

            if ((bool)openFile.ShowDialog()) {
                //ProcessstartInfo bruges til at køre python scriptet.
                ProcessStartInfo info = new ProcessStartInfo();
                List<ElevModel> elever = new List<ElevModel>();

                //Python filen bliver hentet ned til filename fil lokationen.
                info.FileName = RessourceFil.pythonScript;
                //Python scripted bliver hentet og kørt ved hjælp af fileName.
                info.Arguments = $"\"{openFile.FileName}\"";

                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.CreateNoWindow = true;
                info.UseShellExecute = false;

                //processeren bliver kørt, med informationerne fra ProcessStartInfo.
                using (Process process = Process.Start(info)) {
                    using (StreamReader reader = process.StandardOutput) {
                        //data´en fra reader(python) bliver overført til linje(string) en linje adgangen så længe den ikke finde et null.
                        string linje;

                        while ((linje = reader.ReadLine()) != null) {
                            //Data´en fra linje, bliver splittet op i et string array.
                            string[] elev = linje.Split(';');

                            //På denne måde behøver der ikke at tjekkes efter fejl, kun efter om den kan tilføjes.
                            if (elev.Length == 3) {
                                elever.Add(new ElevModel(elev[0], elev[1], elev[2]));
                            }
                            else {
                                AlertBoxes.OnExcelReadingError(linje);
                                return;
                            }
                        }  //While ((linje ...
                    }  //using (StreamReader ...
                }  // using (Process ...

                //Tilføj elever når færdig.
                int aktuelleElevAntal = DbTools.Instance.AntalEleverIAlt;
                DbTools.Instance.TilføjElever(elever);
                AlertBoxes.OnSuccessfulDatabaseInsert(DbTools.Instance.AntalEleverIAlt - aktuelleElevAntal);
            }
        }        
    }
}