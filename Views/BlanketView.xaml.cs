﻿using GFElevInterview.Interfaces;
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
using GFElevInterview.Models;
using System.Linq;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for BlanketView.xaml
    /// </summary>
    public partial class BlanketView : UserControl
    {
        IBlanket currentView;
        DbTools db = new DbTools();
        public BlanketView() {
            InitializeComponent();
            InitializeBlanket();
            db.Database.EnsureCreated();

            //TODO: Debug
            //CurrentElev.elev = db.Elever.FirstOrDefault(x => x.Fornavn.ToLower() == "joakim");
        }

        private void InitializeBlanket() {
            if (currentView == null) {
                currentView = new MeritBlanketView(this);
            }

            mainContent.Content = currentView;
        }

        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (SearchStudentBox.SelectedIndex >= 0) {
                CurrentElev.elev = SearchStudentBox.SelectedItem as ElevModel;
                StudentsFullInfo.Content = CurrentElev.elev.FullInfo;
            }
        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e) {
            string text = SearchStudentTxt.Text;
            SearchStudentBox.ItemsSource = null;
            if(String.IsNullOrEmpty(text))
            {
                return;
            }

            List<ElevModel> elevModels = db.Elever.Where(elev => (elev.Efternavn.ToLower()).StartsWith(text.ToLower()) || elev.CprNr.StartsWith(text)).ToList();
            SearchStudentBox.ItemsSource = elevModels;
        }


        private void Frem_Click(object sender, RoutedEventArgs e) {
            currentView.Frem();
        }

        private void Tilbage_Click(object sender, RoutedEventArgs e) {
            currentView.Tilbage();
        }

        public void ChangeView(IBlanket newView) {
            currentView = newView;
            mainContent.Content = currentView;
        }

        public void UpdateDatabase() {
            db.Elever.Update(CurrentElev.elev);
            db.SaveChanges();
        }
    }
}
