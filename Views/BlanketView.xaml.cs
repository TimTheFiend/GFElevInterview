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
        public BlanketView()
        {
            InitializeComponent();
            InitializeBlanket();
            db.Database.EnsureCreated();

            //TODO: Debug
            CurrentElev.elev = db.Elever.SingleOrDefault(x => x.Fornavn.ToLower() == "joakim");
        }

        private void InitializeBlanket() {
            if (currentView == null) {
                currentView = new MeritBlanketView(this);
            }

            mainContent.Content = currentView;
        }

        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

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
        public void UpdateDatabase()
        {
            db.Elever.Update(CurrentElev.elev);
            db.SaveChanges();
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
