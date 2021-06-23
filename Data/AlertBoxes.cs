using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GFElevInterview.Data
{
    public static class AlertBoxes
    {
        public static void OnSuccessfulCompletion() {
            MessageBox.Show("Success!", "Færdig", MessageBoxButton.OK, MessageBoxImage.None);
        }
    }
}
