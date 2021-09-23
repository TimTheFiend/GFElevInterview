using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GFElevInterview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance = null;  //Singleton

        #region Statisk reference til .xaml elementer

        private static Grid xelOverlayLoading;

        private static TextBlock countBallerup;
        private static TextBlock countFredriksberg;
        private static TextBlock countLyngby;
        private static TextBlock countBallerupPlus;
        private static TextBlock countBallerupFuldt;

        #endregion Statisk reference til .xaml elementer

        public MainWindow() {
            InitializeComponent();
            InitialiserWindow();
        }

        private void InitialiserWindow() {
            //Singleton setup
            if (Instance == null) {
                Instance = this;
            }

            #region Hent ref for xaml elementer

            xelOverlayLoading = overlayLoading;  //Dette gøres for at have en ref i SetBrugerInput.

            countBallerup = txtAntalBal;
            countFredriksberg = txtAntalFred;
            countLyngby = txtAntalLyn;
            countBallerupPlus = txtAntalBalPlus;
            countBallerupFuldt = txtAntalBalFuld;

            #endregion Hent ref for xaml elementer

            SetBrugerInput(true);

            //Viser `BlanketView` ved opstart.
            UnderviserButton_Click(btnUnderviser, new RoutedEventArgs());
        }

        /// <summary>
        /// Sætter visibility status for loading skærm.
        /// </summary>
        /// <param name="harBrugerInput"><c>true</c> hvis brugeren har kontrol; ellers <c>false</c>.</param>
        public static void SetBrugerInput(bool harBrugerInput) {
            xelOverlayLoading.Visibility = harBrugerInput ? Visibility.Collapsed : Visibility.Visible;
        }

        //TODO ordinær+ og fuldtforløb
        public static void OpdaterSkoleOptæller() {
            //var dict = Data.AdminTools.HentAntalEleverPåSkole();
            Dictionary<string, int> skoleAntal = Models.DbTools.Instance.GetAntalEleverPerSkole();  //Placeholder
            try {
                countBallerup.Text = skoleAntal[RessourceFil.ballerup].ToString();
                countFredriksberg.Text = skoleAntal[RessourceFil.frederiksberg].ToString();
                countLyngby.Text = skoleAntal[RessourceFil.lyngby].ToString();

                //NOTE Dum løsning på problemet.
                countBallerupPlus.Text = skoleAntal[RessourceFil.skoleMerit.Substring(0, 3)].ToString();
                countBallerupFuldt.Text = skoleAntal[RessourceFil.skoleIngenMerit.Substring(0, 3)].ToString();
            }
            catch (KeyNotFoundException e) {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        #region Underviser View

        /// <summary>
        /// Viser <see cref="Views.BlanketView"/> viewet.
        /// </summary>
        private void UnderviserButton_Click(object sender, RoutedEventArgs e) {
            //mainContent.Content = new GFElevInterview.Views.maritBlanket();
            mainContent.Content = new Views.BlanketView();
            UnderviserPanel.Visibility = Visibility.Visible;
            OpdaterSkoleOptæller();
        }

        #endregion Underviser View

        #region LederView

        /// <summary>
        /// Viser <see cref="Views.LoginView"/> viewet, før <see cref="Views.LederView"/> bliver vist.
        /// </summary>
        private void LederButton_Click(object sender, RoutedEventArgs e) {
            mainContent.Content = new Views.LoginView(this);
            UnderviserPanel.Visibility = Visibility.Visible;
            OpdaterSkoleOptæller();
        }

        //TODO tænk på bedre løsning.
        public void LoginTilLederView() {
            mainContent.Content = new Views.LederView();
        }

        #endregion LederView

        /// <summary>
        /// Viser <see cref="PLACEHOLDER"/> viewet.
        /// </summary>
        private void VejledningButton_Click(object sender, RoutedEventArgs e) {
            //mainContent.Content = new Views.VejledningsView();
            UnderviserPanel.Visibility = Visibility.Visible;
            OpdaterSkoleOptæller();
        }
    }
}