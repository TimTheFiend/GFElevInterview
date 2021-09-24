using GFElevInterview.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Data
{
    public static class CurrentUser
    {
        public static LoginModel User { get; set; }

        static CurrentUser()
        {
            NulstilCurrentUser();
        }

        //TODO @Victor Doku 
        public static void NulstilCurrentUser()
        {
            User = new LoginModel();
        }

        //TODO @Victor Doku 
        public static bool ErLoggetInd
        {
            get
            {
                return !string.IsNullOrEmpty(User.brugernavn);
            }
        }
    }
}
