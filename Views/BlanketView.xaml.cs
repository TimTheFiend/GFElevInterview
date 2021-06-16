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
        public BlanketView()
        {
            InitializeComponent();
            MeritContent.Content = new GFElevInterview.Views.maritBlanket();
            btnWordView.IsEnabled = false;
        }

        

        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void WordView_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnMeritView_Click(object sender, RoutedEventArgs e)
        {

            MeritContent.Content = new GFElevInterview.Views.WordView();
            btnWordView.IsEnabled = true;
        }

        private void btnWordView_Click(object sender, RoutedEventArgs e)
        {
            MeritContent.Content = new GFElevInterview.Views.maritBlanket();
            
        }
    }
}
