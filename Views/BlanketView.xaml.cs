using GFElevInterview.Interfaces;
using GFElevInterview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        DbTools db = new DbTools();
        public ElevModel currentElev { get; set; } = new ElevModel();

        public BlanketView()
        {
            InitializeComponent();

            if (currentView == null) {
                currentView = new GFElevInterview.Views.MeritBlanketView();
            }


            MeritContent.Content = currentView;

            //btnTilbage.IsEnabled = currentView.Tilbage();

            db.Database.EnsureCreated();
            //btnWordView.IsEnabled = false;
        }


        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SearchStudentBox.SelectedIndex >=0)
            {
                currentElev = SearchStudentBox.SelectedItem as ElevModel;
                StudentsFullInfo.Content = currentElev.FullInfo;
            }
        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = SearchStudentTxt.Text;
            List<ElevModel> elevModels = db.Elever.Where(elev => (elev.Efternavn.ToLower()).StartsWith(text.ToLower()) || elev.Fornavn.ToLower().StartsWith(text.ToLower())).ToList();
            SearchStudentBox.ItemsSource = elevModels;
        }


        private void Frem_Click(object sender, RoutedEventArgs e) {
            //IBlanket _blanket;
            //if (currentView.Frem(out _blanket)) {
            //    currentView = _blanket;
            //    MeritContent.Content = currentView;
            //}
        }

        private void Tilbage_Click(object sender, RoutedEventArgs e) {
            //if (currentView.Frem(out currentView)) {
            //    Console.WriteLine();
            //    MeritContent.Content = currentView;
            //}
        }


        //TODO: implementer ind i Frem_Click
        //private void btnMeritView_Click(object sender, RoutedEventArgs e) {
        //    if (MeritContent.IsEnabled) {
        //        MeritContent.Content = new GFElevInterview.Views.WordView();
        //        btnWordView.IsEnabled = true;

        //        if (WordContent.IsEnabled) {
        //            int age = 30;
        //            if (age < 25) {
        //                //gem pdf file
        //            }
        //            else //åben RKV 
        //            {
        //                WordContent.Content = new GFElevInterview.Views.RkvView();
        //                btnWordView.IsEnabled = true;
        //            }
        //        }
        //    }
        //}
    }
}
