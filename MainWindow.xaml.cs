﻿using System;
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

            Data.AdminTools.HentAntalEleverPåSkole();
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

        #region Lederside
        //todo
        private void btnLeder_Click(object sender, RoutedEventArgs e)
        {
            mainContent.Content = new Views.LederSide();
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;

        }

        //todo
        private void signinButton_Click(object sender, RoutedEventArgs e)
        {
            //GFElevInterview.Views.LederSide lederSide = new Views.LederSide();
            //if (passwordText.Password == "1234")
            //{
            //    lederSide.Show();
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