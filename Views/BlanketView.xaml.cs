using GFElevInterview.Data;
using GFElevInterview.Interfaces;
using GFElevInterview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for BlanketView.xaml
    /// </summary>
    public partial class BlanketView : UserControl
    {
        IBlanket currentView;

        public BlanketView() {
            InitializeComponent();
            InitialiserBlanket();
        }

        private void InitialiserBlanket()
        {
            //TODO Alert Box Yes/No
            CurrentElev.NulstilCurrentElev();
            InitialiserKnapper(false);
        }

        private void InitialiserBlanketView() {
            btnTilbage.IsEnabled = false;

            if (currentView == null) {
                currentView = new MeritBlanketView(this);
            }

            cntMain.Content = currentView;
        }

        private void InitialiserKnapper(bool knapper)
        {
            InitialiserKnapper(knapper, knapper);
        }

        private void InitialiserKnapper(bool frem, bool tilbage)
        {
            btnFrem.IsEnabled = frem;
            btnTilbage.IsEnabled = tilbage;
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
                    MainWindow.Instance.OpdaterCounter();
                    AlertBoxes.OnSuccessfulCompletion();
                }
            }
        }

        private bool OpdaterElevIDatabase() {
            try {
                DbTools.Instance.Elever.Update(CurrentElev.elev);
                DbTools.Instance.SaveChanges();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        //Event Handler

        private void ScrollTilTop()
        {
            scroll.ScrollToTop();
        }
        public void SkiftBlanket(IBlanket newView)
        {
            currentView = newView;
            cntMain.Content = currentView;

            //FIXME kaldes ikke nok
            MainWindow.Instance.OpdaterCounter();
        }

        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (lstSearch.SelectedIndex >= 0) {
                if (CurrentElev.elev.ErUdfyldt) {
                    if (AlertBoxes.OnSelectingNewStudents()) {
                        CurrentElev.NulstilCurrentElev();
                        currentView = null;
                    }
                }
                CurrentElev.elev = lstSearch.SelectedItem as ElevModel;
                lblStudentInfo.Content = CurrentElev.elev.FuldInfo;
                InitialiserBlanketView();

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
            List<ElevModel> elevModels = DbTools.Instance.Elever.Where(
                elev => (elev.efternavn.ToLower()).StartsWith(text.ToLower())
                || elev.cprNr.StartsWith(text)
                || elev.fornavn.ToLower().StartsWith(text.ToLower())
                ).ToList();
            lstSearch.ItemsSource = elevModels;
        }


        private void Frem_Click(object sender, RoutedEventArgs e) {
            currentView.Frem();
            ScrollTilTop();
        }

        private void Tilbage_Click(object sender, RoutedEventArgs e) {
            currentView.Tilbage();
            ScrollTilTop();
        }


    }
}