using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using GFElevInterview.Models;


namespace GFElevInterview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            HomePanel.Visibility = Visibility.Visible;
            UnderviserPanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
        }

        private void btnUnderviser_Click(object sender, RoutedEventArgs e)
        {
            mainContent.Content = new GFElevInterview.Views.maritBlanket();
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
           LederPanel.Visibility = Visibility.Collapsed;
        }

        private void btnLeder_Click(object sender, RoutedEventArgs e)
        {
            LederPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            UnderviserPanel.Visibility = Visibility.Collapsed;
        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void signinButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void passwordText_KeyDown(object sender, KeyEventArgs e)
        {
        
        }
    }
}
