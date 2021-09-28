using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO.Compression;
using System.Linq;
using System.Diagnostics;
using Microsoft.Win32;
using GFElevInterview.Models;

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
                //TODO Alertbox
            }
            return filer;
        }

        /// <summary>
        /// Kombinerer alle merit-blanketter ind i én .PDF-fil.
        /// </summary>
        public static void KombinerMeritFiler() {
            List<string> filNavne = HentFiler(RessourceFil.endMerit);

            Document nytDokument = new Document();
            using (FileStream fs = new FileStream(RessourceFil.samletMerit, FileMode.Create)) {
                PdfCopy writer = new PdfCopy(nytDokument, fs);
                if (writer == null) {
                    //TODO fejlmeddelse
                    return;
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
            }
        }

        /// <summary>
        /// Zipper alle RKV-blanketter ind i én zip-fil.
        /// </summary>
        public static void ZipRKVFiler() {
            List<string> filNavne = HentFiler(RessourceFil.endRKV);

            string filSti = RessourceFil.samletRKV;
            if (File.Exists(filSti)) {
                File.Delete(filSti);
            }

            var zip = ZipFile.Open(filSti, ZipArchiveMode.Create);
            foreach (string filNavn in filNavne) {
                zip.CreateEntryFromFile(filNavn, Path.GetFileName(filNavn), CompressionLevel.Optimal);
            }
            zip.Dispose();
        }

        /// <summary>
        /// Åbner en ny instans af Explorer med den valgte fil selected.
        /// </summary>
        public static void VisBlanketIExplorer(string filNavn) {
            string curDir = Directory.GetCurrentDirectory();
            int index = curDir.LastIndexOf('\\');

            string filSti = Path.Combine(curDir.Substring(0, index), RessourceFil.outputMappeNavn, filNavn);
            Process.Start("explorer.exe", $"/select,\"{filSti}");  //"/select," highlighter den valgte fil.
        }

        public static void OpenFileDialog() {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            openFile.Filter = "Excel File |*.xls;*.xlsx;*.xlsm";

            Nullable<bool> result = openFile.ShowDialog();
            if ((bool)result) {
                //ProcessstartInfo bruges til at køre python scriptet.
                ProcessStartInfo info = new ProcessStartInfo();
                List<ElevModel> elever = new List<ElevModel>();

                //Python filen bliver hentet ned til filename fil lokationen.
                //info.FileName = ".venv\\Scripts\\python.exe";
                info.FileName = RessourceFil.pythonScript;
                //Python scripted bliver hentet og kørt ved hjælp af fileName.
                info.Arguments = string.Format("\"{0}\"", openFile.FileName);

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
                            string[] elev = linje.Split(';');  //Note: hardCoded seperator
                            if (elev.Length < 2) {
                                AlertBoxes.OnExcelReadingError(linje);
                                return;
                            }
                            //Data´en fra String Array´et bliver tilføjet til elev listen.
                            elever.Add(new ElevModel(elev[0], elev[1], elev[2]));
                        }
                    }
                }
                //DbTools TilføjElever bliver kaldt, hvorefter at eleverne bliver tilføjet til databasen.
                DbTools.Instance.TilføjElever(elever);
            }
        }
    }
}