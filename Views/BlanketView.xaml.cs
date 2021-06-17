using GFElevInterview.Interfaces;
using GFElevInterview.Data;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for BlanketView.xaml
    /// </summary>
    public partial class BlanketView : UserControl
    {
        IBlanket currentView;
        public BlanketView()
        {
            InitializeComponent();

            if (currentView == null) {
                currentView = new GFElevInterview.Views.MeritBlanketView();
            }

            MeritContent.Content = currentView;

            IBlanket _;
            btnTilbage.IsEnabled = currentView.Tilbage(out _);

            //btnWordView.IsEnabled = false;
        }

        

        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void Frem_Click(object sender, RoutedEventArgs e)
        {
            IBlanket _blanket;
            if (currentView.Frem(out _blanket)) {
                currentView = _blanket;
                MeritContent.Content = currentView;
                UdprintMerit print = new UdprintMerit();
                print.udprintFraWord();
            }
        }

        private void Tilbage_Click(object sender, RoutedEventArgs e)
        {
            if (currentView.Frem(out currentView)) {
                Console.WriteLine();
                MeritContent.Content = currentView;
            }
        }
    }
}
