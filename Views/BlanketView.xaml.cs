using GFElevInterview.Interfaces;
using GFElevInterview.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GFElevInterview.Models;
using System.Linq;
using System.Threading.Tasks;


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

            CurrentElev.NulstilCurrentElev();
        }

        private void InitialiserBlanket() {
            if (currentView == null) {
                currentView = new MeritBlanketView(this);
            }

            cntMain.Content = currentView;
        }

        public void FærdiggørInterview() {

            bool? isRKVSuccess = null;
            bool isMeritSuccess = false;

            //CurrentElev.elev.BeregnMeritIUger(CurrentElev.elev);

            BlanketUdskrivning print = new BlanketUdskrivning();

            ///Task
            Task meritTask = Task.Run(() => {
                isMeritSuccess = print.UdskrivningMerit();
            });

            while (!meritTask.IsCompleted) { }

            //Hvis merit er blevet udskrevet, og RKV enten også er, eller slet ikke (fordi eleven ikke er RKV), så opdater databasen.
            if (isMeritSuccess && (isRKVSuccess == null || isRKVSuccess == true)) {
                if (OpdaterElevIDatabase()) {
                    CurrentElev.NulstilCurrentElev();
                    //TODO Lav metode to reset
                    currentView = null;
                    cntMain.Content = null;
                    lblStudentInfo.Content = "";
                    MainWindow.instance.OpdaterCounter();
                    AlertBoxes.OnSuccessfulCompletion();
                }
            }
        }

        private bool OpdaterElevIDatabase()
        {
            try
            {
                db.Elever.Update(CurrentElev.elev);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //Event Handler

        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (lstSearch.SelectedIndex >= 0) {
                if(CurrentElev.elev.ErUdfyldt)
                {
                    if (AlertBoxes.OnSelectingNewStudents())
                    {
                        CurrentElev.NulstilCurrentElev();
                        currentView = null;
                    }
                }
                InitialiserBlanket();
                CurrentElev.elev = lstSearch.SelectedItem as ElevModel;
                lblStudentInfo.Content = CurrentElev.elev.FuldInfo;

                //Nulstiller textbox og listbox
                txtSearch.Text = "";
                lstSearch.ItemsSource = null;
            }
        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e) {
            string text = txtSearch.Text;
            lstSearch.ItemsSource = null;
            if (String.IsNullOrEmpty(text)) {
                return;
            }
            //TODO Ryk til DbTools
            List<ElevModel> elevModels = db.Elever.Where(
                elev => (elev.efternavn.ToLower()).StartsWith(text.ToLower())
                || elev.cprNr.StartsWith(text)
                || elev.fornavn.ToLower().StartsWith(text.ToLower())
                ).ToList();
            lstSearch.ItemsSource = elevModels;
        }

        private void OnButtonClick() {
            scroll.ScrollToTop();
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
            cntMain.Content = currentView;

            //FIXME kaldes ikke nok
            MainWindow.instance.OpdaterCounter();
        }
    }
}