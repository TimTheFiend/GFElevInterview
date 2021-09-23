using System.Windows;

namespace GFElevInterview.Data
{
    /// <summary>
    /// En statisk klasse der indeholder forskellige <see cref="MessageBox"/> variationer til forskellige situationer.
    /// </summary>
    public static class AlertBoxes
    {
        /* readonly Fields for konsistent udskrivning. */
        private static readonly string msgError = "Fejl!";
        private static readonly string msgWarning = "Advarsel!";

        /* Fields */
        private static string breadText = "Placeholder text, shouldn't be seen by users.";
        private static string captionText = "Placeholder text, shouldn't be seen by users.";

        #region AlertBoxes med én knap

        /// <summary>
        /// Instantiere Alertboxen til brugeren.
        /// </summary>
        private static void ShowAlertBox() {
            MessageBox.Show(breadText, captionText, MessageBoxButton.OK);
        }

        /// <summary>
        /// Viser en besked om at interviewet er færdiggjort, og at informationen er blevet gemt, og udskrevet.
        /// </summary>
        public static void OnFinishedInterview() {
            breadText = "Udskrivning af blanket(ter) er færdig!";
            captionText = "Succesfuld udskrivning!";

            ShowAlertBox();
        }

        /// <summary>
        /// Viser en besked der fortæller brugeren at udprintning af blanket(ter) er startet.
        /// </summary>
        public static void OnStartPrintingPDF() {
            breadText = "Udskrivning af blanket(ter) er påbegyndt.";
            captionText = "Udskrivning startet!";

            ShowAlertBox();
        }

        //NOTE Ikke i brug
        public static void OnOpenFileFailure() {
            breadText = "Den valgte blanket findes ikke i mappen.";
            captionText = msgError;

            ShowAlertBox();
        }

        /// <summary>
        /// Viser en besked med brugerdefineret fejl fra Python.
        /// </summary>
        /// <param name="errorMessage"></param>
        public static void OnExcelReadingError(string errorMessage) {
            breadText = errorMessage;
            captionText = msgError;

            ShowAlertBox();
        }

        //TODO @Victor Doku og udvidelse
        public static void OnFailedLoginAttempt() {
            breadText = "Forkert password, adgang nægtet.";
            captionText = msgError;

            ShowAlertBox();
        }

        public static void OnFailedMatchAttempt()
        {
            breadText = "Password´ene passede ikke, prøv igen.";
            captionText = msgError;

            ShowAlertBox();
        }
        /// <summary>
        /// Som navnet hentyder til, så burde denne fejlboks aldrig blive vist.
        /// Den eksisterer med det eneste formål at fortælle brugeren at de skal kontakte devs.
        /// </summary>
        public static void OnUnlikelyError() {
            breadText = "Der er sket en uforventet fejl.\n" +
                "Noter venligst hvad der skete op til denne boks blev vist, og kontakt programmørerne.";
            captionText = msgError;

            //TODO tilføj reference til "credits" i vejledning.
            ShowAlertBox();
        }

        #endregion AlertBoxes med én knap

        #region AlertBoxes med to knapper

        /// <summary>
        /// Viser en besked som spørger om Leder er sikker på at fortsætte med handling.
        /// </summary>
        /// <returns><c>true</c> hvis de vælger "Ja"; ellers <c>false</c>.</returns>
        private static bool ShowYesNoAlertBox() {
            MessageBoxResult result = MessageBox.Show(breadText, captionText, MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Viser en besked som spørger om Leder er sikker på at fortsætte exportering af merit dokumenter.
        /// </summary>
        /// <returns><c>true</c> hvis de vælger "Ja"; ellers <c>false</c>.</returns>
        public static bool OnExportMerit() {
            breadText = "Er du sikker på at sammensætte merit-dokumenterne?";
            captionText = "Export";

            return ShowYesNoAlertBox();
        }

        /// <summary>
        /// Viser en besked som spørger om Leder er sikker på at fortsætte exportering af RKV dokumenter.
        /// </summary>
        /// <returns><c>true</c> hvis de vælger "Ja"; ellers <c>false</c>.</returns>
        public static bool OnExportRKV() {
            breadText = "Er du sikker på at zippe RKV-dokumenterne?";
            captionText = "Export";

            return ShowYesNoAlertBox();
        }

        /// <summary>
        /// Viser en besked som spørger om underviser er sikker på at skifte elev hvis interview allerede er påbegyndt.
        /// </summary>
        /// <returns><c>true</c> hvis de vælger "Ja"; ellers <c>false</c>.</returns>
        public static bool OnSelectingNewStudent() {
            breadText = "Er du sikker på at skifte til en ny elev?";
            captionText = msgWarning;

            return ShowYesNoAlertBox();
        }

        #endregion AlertBoxes med to knapper
    }
}