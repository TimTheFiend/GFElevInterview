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
        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext =;
            //Data.AdminTools.HentAntalEleverPåSkole();

            //new DbTools().TilføjElever();
            KaldPy();
            OpdaterCounter();
        }

        private void KaldPy()
        {
            string exe = @"C:\Python39\python.exe";
            string script = @"C:\Users\viga\Documents\GitHub\GFElevInterview\bin\Debug\Blanketter\py.py";
            string attempt = @"C:\Users\viga\Documents\GitHub\GFElevInterview\bin\Debug\Blanketter\2211334455 - Baghave, Blomsterkasse_RKV.pdf";
            System.Diagnostics.Process.Start(exe, $"{script} \"{attempt}\"");
        }

        private void OpdaterCounter()
        {
            var dict = Data.AdminTools.HentAntalEleverPåSkole();
            LyngbyTXT.Text = dict["Lyngby"].ToString();
            BallerupTXT.Text = dict["Ballerup"].ToString();
            FredriksbergTXT.Text = dict["Frederiksberg"].ToString();
        }
        #region Home
        //TODO overvej at fjerne?
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            HomePanel.Visibility = Visibility.Visible;
            UnderviserPanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Underviser View
        private void btnUnderviser_Click(object sender, RoutedEventArgs e)
        {
            //mainContent.Content = new GFElevInterview.Views.maritBlanket();
            mainContent.Content = new Views.BlanketView();
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region LederView
        //todo
        private void btnLeder_Click(object sender, RoutedEventArgs e)
        {
            mainContent.Content = new Views.LederView();
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;

        }

        //todo
        private void signinButton_Click(object sender, RoutedEventArgs e)
        {
            //GFElevInterview.Views.LederView LederView = new Views.LederView();
            //if (passwordText.Password == "1234")
            //{
            //    LederView.Show();
            //    this.Close();
            //}
            //else if (passwordText.Password == "")
            //{
            //    MessageBox.Show("Indtast adgangskode!!");
            //    passwordText.Focus();
            //}
            //else
            //{
            //    MessageBox.Show("Ugyldig adgangskode!!");
            //    passwordText.Clear();
            //    passwordText.Focus();
            //}
        }

        private void passwordText_KeyDown(object sender, KeyEventArgs e)
        {
        }

        #endregion
    }
}