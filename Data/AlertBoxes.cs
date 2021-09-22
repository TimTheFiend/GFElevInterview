using System.Windows;

namespace GFElevInterview.Data
{
    /// <summary>
    /// Forsøg på at lave en statisk klasse der indeholder forskellige <see cref="MessageBox"/> variationer til forskellige situationer.
    /// Blev aldrig implementeret, da jeg (Joakim) lavede den spontant og opdagede at det var unødvendigt.
    /// </summary>
    public static class AlertBoxes
    {
        /// <summary>
        /// Viser en besked om at interviewet er færdiggjort, og at informationen er blevet gemt, og udskrevet.
        /// </summary>
        public static void OnFinishedInterview() {
            MessageBox.Show("Udskrivning af blanket(ter) er færdig!", "Succesfuld udskrivning", MessageBoxButton.OK);
        }

        /// <summary>
        /// Viser en besked der fortæller brugeren at udprintning af blanket(ter) er startet.
        /// </summary>
        public static void OnStartPrintingPDF() {
            MessageBox.Show("Udskrivning af blanket(ter) er påbegyndt.", "Udskrivning startet!", MessageBoxButton.OK);
        }

        public static void OnOpenFileFailure() {
            MessageBox.Show("Den valgte blanket findes ikke i mappen", "Fejl!", MessageBoxButton.OK);
        }

        /// <summary>
        /// Viser en besked med brugerdefineret fejl fra Python.
        /// </summary>
        /// <param name="errorMessage"></param>
        public static void OnExcelReadingError(string errorMessage) {
            MessageBox.Show(errorMessage, "Fejl i læsning af PDF!", MessageBoxButton.OK);
        }

        /// <summary>
        /// Viser en besked som spørger om Leder er sikker på at fortsætte med handling.
        /// </summary>
        /// <returns><c>true</c> hvis de vælger "Ja"; ellers <c>false</c>.</returns>
        public static bool OnExport() {
            MessageBoxResult result = MessageBox.Show("Er du sikker?", "Export", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Viser en besked som spørger om underviser er sikker på at skifte elev hvis interview allerede er påbegyndt.
        /// </summary>
        /// <returns><c>true</c> hvis de vælger "Ja"; ellers <c>false</c>.</returns>
        public static bool OnSelectingNewStudent() {
            MessageBoxResult result = MessageBox.Show("Er du sikker på at skifte til en ny elev?", "ADVARSEL", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }

        public static void OnFailedLoginAttempt()
        {
            MessageBox.Show("Password er forkert!!");
        }
    }
}