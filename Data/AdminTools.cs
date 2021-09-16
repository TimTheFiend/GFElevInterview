using GFElevInterview.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using config = System.Configuration.ConfigurationManager;

namespace GFElevInterview.Data
{
    public static class AdminTools
    {
        //TODO DOKU
        public static void KombinerMeritFiler() {
            string[] filNavne = HentFiler(config.AppSettings.Get("endMerit"));

            Document nytDokument = new Document();
            using (FileStream fs = new FileStream(config.AppSettings.Get("samletMerit"), FileMode.Create)) {
                PdfCopy writer = new PdfCopy(nytDokument, fs);
                if (writer == null)
                    return;

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

        public static void ZipRKVFiler() {
            string[] filNavne = HentFiler(config.AppSettings.Get("endRKV"));

            string filSti = config.AppSettings.Get("samletRKV");
            if (File.Exists(filSti)) {
                File.Delete(filSti);
            }

            var zip = ZipFile.Open(filSti, ZipArchiveMode.Create);
            foreach (string filNavn in filNavne) {
                zip.CreateEntryFromFile(filNavn, System.IO.Path.GetFileName(filNavn), CompressionLevel.Optimal);
            }
            zip.Dispose();
        }


        public static string[] HentFiler(string endelse) {
            string[] filer = Directory.GetFiles(config.AppSettings.Get("outputMappe"), $"*{endelse}");
            return filer;
        }

        public static Dictionary<string, int> HentAntalEleverPåSkole() {
            Dictionary<string, int> antalElever = new Dictionary<string, int>();

            List<string> skoler = (from e in DbTools.Instance.Elever
                                   select e.uddannelseAdresse).ToList();

            string ballerupAntal = config.AppSettings.Get("ballerup");
            string frederiksbergAntal = config.AppSettings.Get("frederiksberg");
            string lyngbyAntal = config.AppSettings.Get("lyngby");
            antalElever.Add(ballerupAntal, skoler.Where(x => x == ballerupAntal).ToList().Count);
            antalElever.Add(frederiksbergAntal, skoler.Where(x => x == frederiksbergAntal).ToList().Count);
            antalElever.Add(lyngbyAntal, skoler.Where(x => x == lyngbyAntal).ToList().Count);


            return antalElever;
        }
    }
}
