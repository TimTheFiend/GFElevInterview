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
using System.Threading.Tasks;
using System.Threading;


namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for BlanketView.xaml
    /// </summary>
    public partial class BlanketView : UserControl {
        IBlanket currentView;
        DbTools db = new DbTools();

        public BlanketView() {
            InitializeComponent();

            CurrentElev.ResetCurrentElev();
            //InitializeBlanket();
            //db.Database.EnsureCreated();

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
                if(CurrentElev.meritBlanket.IsFilled)
                {
                    MessageBoxResult result = MessageBox.Show("Er du sikkert at ville skifte elev?", "TEMP TEXT", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        CurrentElev.ResetCurrentElev();
                        currentView = null;
                    }
                }
                InitializeBlanket();
                CurrentElev.elev = SearchStudentBox.SelectedItem as ElevModel;
                StudentsFullInfo.Content = CurrentElev.elev.FullInfo;

                //Nulstiller textbox og listbox
                SearchStudentTxt.Text = "";
                SearchStudentBox.ItemsSource = null;
            }
        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e) {
            string text = SearchStudentTxt.Text;
            SearchStudentBox.ItemsSource = null;
            if (String.IsNullOrEmpty(text)) {
                return;
            }

            List<ElevModel> elevModels = db.Elever.Where(
                elev => (elev.Efternavn.ToLower()).StartsWith(text.ToLower())
                || elev.CprNr.StartsWith(text)
                || elev.Fornavn.ToLower().StartsWith(text.ToLower())
                ).ToList();
            SearchStudentBox.ItemsSource = elevModels;
        }

        private void OnButtonClick() {
            scrollview.ScrollToTop();
        }
        private void Frem_Click(object sender, RoutedEventArgs e) {
            currentView.Frem();
            OnButtonClick();
        }

        private void Tilbage_Click(object sender, RoutedEventArgs e) {
            currentView.Tilbage();
            OnButtonClick();
        }

        public void ChangeView(IBlanket newView) {
            currentView = newView;
            mainContent.Content = currentView;
        }

        private bool UpdateDatabase() {
            try {
                db.Elever.Update(CurrentElev.elev);
                db.SaveChanges();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        public void CompleteCurrentInterview() {

            bool? isRKVSuccess = null;
            bool isMeritSuccess = false;

            CurrentElev.meritBlanket.BeregnMeritIUger(CurrentElev.elev);

            BlanketUdskrivning print = new BlanketUdskrivning();

            ///Task
            Task meritTask = Task.Run(() => {
                isMeritSuccess = print.UdskrivningMerit();
            });

            while (!meritTask.IsCompleted) { }

            //Hvis merit er blevet udskrevet, og RKV enten også er, eller slet ikke (fordi eleven ikke er RKV), så opdater databasen.
            if (isMeritSuccess && (isRKVSuccess == null || isRKVSuccess == true)) {
                if (UpdateDatabase()) {
                    CurrentElev.ResetCurrentElev();
                    currentView = null;
                    mainContent.Content = null;
                    StudentsFullInfo.Content = "";
                    AlertBoxes.OnSuccessfulCompletion();
                }
            }
        }
    }
}