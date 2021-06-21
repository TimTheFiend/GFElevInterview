using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GFElevInterview.Services
{
    public class AlertsBoxes
    {
        public static bool SelectingNewElevAlert() {
            MessageBoxResult result = MessageBox.Show(
                                    messageBoxText: "Du har allerede valgt en elev.\nDu vil nulstille allerede valgt data hvis du fortsætter.",
                                    caption: "Hey Ya",
                                    button: MessageBoxButton.YesNo,
                                    icon: MessageBoxImage.Information
                                );

            if (result == MessageBoxResult.Yes) {
                Console.WriteLine();
                return true;
            }

            return false;
        }
    }
}
