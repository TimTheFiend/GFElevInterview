using GFElevInterview.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Data
{
    public static class CurrentUser
    {
        public static LoginModel User { get; set; }

        static CurrentUser() {
            NulstilCurrentUser();
        }

        /// <summary>
        /// NulStiller CurrentUser hvis kaldet.
        /// </summary>
        public static void NulstilCurrentUser() {
            User = new LoginModel();
        }

        /// <summary>
        /// Checker om CurrentUser er loget ind eller ej.
        /// </summary>
        public static bool ErLoggetInd {
            get {
                return !string.IsNullOrEmpty(User.brugernavn);
            }
        }
    }
}