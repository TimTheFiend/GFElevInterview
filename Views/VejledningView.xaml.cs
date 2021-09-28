using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for VejledningView.xaml
    /// </summary>
    public partial class VejledningView : UserControl
    {
        public VejledningView() {
            InitializeComponent();
        }
        private void ÅbenFilPlacering(string blanketNavn)
        {
            string currentDir = Directory.GetCurrentDirectory();
            int index = currentDir.LastIndexOf('\\');  //finder positionen på sidste "\" i current dir
            //kombinerer strings til at give os filstien på den valgte pdf
            string filSti = System.IO.Path.Combine(currentDir.Substring(0, index), RessourceFil.outputMappeTest, blanketNavn);

            Process.Start("explorer.exe", $"/select,\"{filSti}");  //"/select," highlighter den valgte fil.
        }

        private void buttonLeder_Click(object sender, RoutedEventArgs e)
        {
            ÅbenFilPlacering("Brugervejledning (Leder).pdf");
        }

        private void buttonVej_Click(object sender, RoutedEventArgs e)
        {
            ÅbenFilPlacering("Brugervejledning.pdf");
        }
    }
}
