using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
    }
}