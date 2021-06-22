using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Data
{
    public class BlanketUdskrivning
    {

        public static bool UdskrivningRKV()
        {
            string inputFil = GetFile(); 

            
            return false;
        }

        private static string GetFile()
        {
            string foo = "EUV1 - Infrastruktur.pdf";
            return "Blanketter\\Templates\\" + foo;
        }
    }
}
