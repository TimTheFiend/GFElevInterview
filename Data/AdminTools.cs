using GFElevInterview.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace GFElevInterview.Data
{
    public static class AdminTools
    {
        /// <summary>
        /// Kombinerer alle merit-blanketter ind i én .PDF-fil.
        /// </summary>
        public static void KombinerMeritFiler() {
            string[] filNavne = HentFiler(RessourceFil.endMerit);

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
                writer.Close();
                nytDokument.Close();
            }
        }

        /// <summary>
        /// Zipper alle RKV-blanketter ind i én zip-fil.
        /// </summary>
        public static void ZipRKVFiler() {
            string[] filNavne = HentFiler(RessourceFil.endRKV);

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
        /// Henter en liste af relevante filer i <see cref="RessourceFil.outputMappeNavn"/> mappen.
        /// </summary>
        /// <param name="endelse">Endelsen på filerne der skal hentes.</param>
        /// <returns><see cref="List"/> af filers filsti.</returns>
        public static string[] HentFiler(string endelse) {
            string[] filer = Directory.GetFiles(RessourceFil.outputMappe, $"*{endelse}");
            return filer;
        }

        //TODO udvid funktionalitet
        public static Dictionary<string, int> HentAntalEleverPåSkole() {
            Dictionary<string, int> antalElever = new Dictionary<string, int>();

            List<string> skoler = (from e in DbTools.Instance.Elever
                                   select e.uddannelseAdresse).ToList();

            string ballerupAntal = RessourceFil.ballerup;
            string frederiksbergAntal = RessourceFil.frederiksberg;
            string lyngbyAntal = RessourceFil.lyngby;
            antalElever.Add(ballerupAntal, skoler.Where(x => x == ballerupAntal).ToList().Count);
            antalElever.Add(frederiksbergAntal, skoler.Where(x => x == frederiksbergAntal).ToList().Count);
            antalElever.Add(lyngbyAntal, skoler.Where(x => x == lyngbyAntal).ToList().Count);

            return antalElever;
        }
    }
}