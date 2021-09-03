using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO.Compression;
using config = System.Configuration.ConfigurationManager;
using GFElevInterview.Models;

namespace GFElevInterview.Data
{
    public static class AdminTools
    {
        //TODO DOKU
        public static void KombinerMeritFiler()
        {
            string[] filNavne = HentFiler(config.AppSettings.Get("endMerit"));

            Document nytDokument = new Document();
            using (FileStream fs = new FileStream(config.AppSettings.Get("samletMerit"), FileMode.Create))
            {
                PdfCopy writer = new PdfCopy(nytDokument, fs);
                if (writer == null)
                    return;

                nytDokument.Open();

                foreach (string filNavn in filNavne)
                {
                    PdfReader reader = new PdfReader(filNavn);

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }
                    reader.Close();
                }
                writer.Close();
                nytDokument.Close();
            }
        }

        public static void ZipRKVFiler()
        {
            string[] filNavne = HentFiler(config.AppSettings.Get("endRKV"));

            string filSti = config.AppSettings.Get("samletRKV");
            if (File.Exists(filSti))
            {
                File.Delete(filSti);
            }

            var zip = ZipFile.Open(filSti, ZipArchiveMode.Create);
            foreach (string filNavn in filNavne)
            {
                zip.CreateEntryFromFile(filNavn, System.IO.Path.GetFileName(filNavn), CompressionLevel.Optimal);
            }
            zip.Dispose();
        }


        private static string[] HentFiler(string endelse)
        {
            string[] filer = Directory.GetFiles(config.AppSettings.Get("outputMappe"), $"*{endelse}");
            return filer;
        }

        public static void HentAntalEleverPåSkole()
        {
            DbTools db = new DbTools();

            int ballerupTotal = db.Elever.Where(x => x.uddannelseAdresse == config.AppSettings.Get("ballerup")).ToList().Count;
            int frederiksberg = db.Elever.Where(x => x.uddannelseAdresse == config.AppSettings.Get("frederiksberg")).ToList().Count;
            int lyngby = db.Elever.Where(x => x.uddannelseAdresse == config.AppSettings.Get("lyngby")).ToList().Count;
            int ballerupMerit = db.Elever.Where(x => x.uddannelseAdresse == config.AppSettings.Get("ballerup") && x.danskNiveau > FagNiveau.F).ToList().Count;
            int ballerupFuldt = db.Elever.Where(x => x.uddannelseAdresse == config.AppSettings.Get("ballerup") && x.danskNiveau < FagNiveau.E).ToList().Count;

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
