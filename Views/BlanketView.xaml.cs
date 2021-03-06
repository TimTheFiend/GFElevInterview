using GFElevInterview.Data;
using GFElevInterview.Interfaces;
using GFElevInterview.Models;
using GFElevInterview.Tools;
using System;
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
        private IBlanket currentView;

        public BlanketView() {
            InitializeComponent();
            InitialiserBlanket();
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            txtSearch.Focus();
        }

        private void InitialiserBlanket() {
            CurrentElev.NulstilCurrentElev();
            InitialiserKnapper(false);
        }

        /// <summary>
        /// Kaldes når Elev er valgt fra Søgefeldt.
        /// </summary>
        private void InitialiserBlanketView() {
            btnTilbage.IsEnabled = false;

            if (currentView == null) {
                currentView = new MeritBlanketView(this);
            }
            //cntList.Content = lstSearch;
            cntMain.Content = currentView;
        }

        /// <summary>
        /// Starter <see cref="TaskFærdiggørInterview"/> som en <see cref="Task"/>, og fortæller brugeren at processen er startet.
        /// </summary>
        public void FærdiggørInterview() {
            Task.Run(() => {
                TaskFærdiggørInterview();
            });
            MainWindow.SetBrugerInput(false);
            AlertBoxes.OnStartPrintingPDF();
        }

        /// <summary>
        /// Sætter `IsEnabled` på <see cref="Button"/>s.
        /// </summary>
        /// <param name="knapper">`IsEnabled` status.</param>
        private void InitialiserKnapper(bool knapper) {
            InitialiserKnapper(knapper, knapper);
        }

        /// <summary>
        /// Sætter `IsEnabled` på <see cref="btnFrem"/> og <see cref="btnTilbage"/>.
        /// </summary>
        /// <param name="frem"><see cref="btnFrem"/>s status</param>
        /// <param name="tilbage"><see cref="btnTilbage"/>s status</param>
        private void InitialiserKnapper(bool frem, bool tilbage) {
            btnFrem.IsEnabled = frem;
            btnTilbage.IsEnabled = tilbage;
        }

        /// <summary>
        /// Kaldes som en <see cref="Task"/>, og sørger for udprintning, og nulstilling af GUI,
        /// og at informere brugeren når udprintningen er færdiggjort.
        /// </summary>
        private void TaskFærdiggørInterview() {
            bool? isRKVSuccess = null;
            bool isMeritSuccess = false;

            isMeritSuccess = BlanketUdskrivning.UdskrivningMerit();

            //Hvis merit er blevet udskrevet, og RKV enten også er, eller slet ikke (fordi eleven ikke er RKV), så opdater databasen.
            if (isMeritSuccess && (isRKVSuccess == null || isRKVSuccess == true)) {
                if (OpdaterElevIDatabase()) {
                    CurrentElev.NulstilCurrentElev();

                    this.Dispatcher.Invoke(() => {
                        NulstilBlanketView();
                        MainWindow.SetBrugerInput(true);
                    });

                    AlertBoxes.OnFinishedInterview();
                }
            }
        }

        /// <summary>
        /// Nulstillingens-funktion efter færdiggjort interview.
        /// </summary>
        private void NulstilBlanketView() {
            currentView = null;
            cntMain.Content = null;
            lblStudentInfo.Content = "";

            MainWindow.OpdaterSkoleOptæller();
            InitialiserKnapper(false);
        }

        private bool OpdaterElevIDatabase() {
            try {
                DbTools.Instance.OpdaterElevData(CurrentElev.elev);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Scroller indholdet i <see cref="cntMain"/> op til tops.
        /// </summary>
        private void ScrollTilTop() {
            scroll.ScrollToTop();
        }

        /// <summary>
        /// Udskrifter indhold i <see cref="cntMain"/>.
        /// </summary>
        /// <param name="newView">Det nye view der skal vises.</param>
        public void SkiftBlanket(IBlanket newView) {
            currentView = newView;
            cntMain.Content = currentView;

            //FIXME kaldes ikke nok
            MainWindow.OpdaterSkoleOptæller();
        }

        /// <summary>
        /// EventHandler efter en elev er blevet valgt i søgefeldtet.
        /// </summary>
        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (lstSearch.SelectedIndex >= 0) {
                if (CurrentElev.elev.ErUdfyldt) {
                    if (AlertBoxes.OnSelectingNewStudent()) {
                        CurrentElev.NulstilCurrentElev();
                        currentView = null;
                    }
                }
                CurrentElev.elev = lstSearch.SelectedItem as ElevModel;
                lblStudentInfo.Content = CurrentElev.elev.ToString();
                InitialiserBlanketView();

                //Nulstiller textbox og listbox
                txtSearch.Text = "";
                lstSearch.ItemsSource = null;
            }
        }

        /// <summary>
        /// EventHandler når der indtastes noget i <see cref="txtSearch"/>.
        /// </summary>
        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e) {
            string query = (sender as TextBox).Text;
            lstSearch.ItemsSource = null;

            if (string.IsNullOrEmpty(query)) {
                return;
            }

            lstSearch.ItemsSource = DbTools.Instance.VisQueryElever(query);
        }

        /// <summary>
        /// Håndtering af <see cref="btnFrem"/> funktionalitet.
        /// </summary>
        private void Frem_Click(object sender, RoutedEventArgs e) {
            if (currentView != null) {
                currentView.Frem();
                ScrollTilTop();
            }
        }

        /// <summary>
        /// Håndtering af <see cref="btnTilbage"/> funktionalitet.
        /// </summary>
        private void Tilbage_Click(object sender, RoutedEventArgs e) {
            if (currentView != null) {
                currentView.Tilbage();
                ScrollTilTop();
            }
        }

        /// <summary>
        /// Viser en liste af alle elever.
        /// </summary>
        private void VisAlle_Click(object sender, RoutedEventArgs e) {
            lstSearch.ItemsSource = DbTools.Instance.VisAlle();
        }
    }
}