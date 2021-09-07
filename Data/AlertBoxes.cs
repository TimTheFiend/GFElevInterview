using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GFElevInterview.Data
{
    /// <summary>
    /// Forsøg på at lave en statisk klasse der indeholder forskellige <see cref="MessageBox"/> variationer til forskellige situationer.
    /// Blev aldrig implementeret, da jeg (Joakim) lavede den spontant og opdagede at det var unødvendigt.
    /// </summary>
    public static class AlertBoxes
    {
        public static void OnSuccessfulCompletion() {
            MessageBox.Show("Success!", "Færdig", MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static void OnOpenFileFailure()
        {
            MessageBox.Show("Den valgte blanket findes ikke i mappen", "Fejl!", MessageBoxButton.OK);
        }

        public static bool OnExport()
        {
            MessageBoxResult result = MessageBox.Show("er du sikker?", "Export", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes ? true : false;
        }

        public static bool OnSelectingNewStudents()
        {
            MessageBoxResult result = MessageBox.Show("Er du sikkert at ville skifte elev?", "ADVARSEL", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes ? true : false;
        }
    }
}