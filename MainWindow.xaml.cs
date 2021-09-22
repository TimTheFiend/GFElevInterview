using System.Windows;

namespace GFElevInterview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance = null;

        public MainWindow() {
            InitializeComponent();
            if (Instance == null) {
                Instance = this;
            }
            ÅbenUndervisning();
            //this.DataContext =;
            //Data.AdminTools.HentAntalEleverPåSkole();

            //new DbTools().TilføjElever();
            new Models.LoginModel().ChangePassword("helloworld");

            OpdaterCounter();
        }

        private void ÅbenUndervisning() {
            mainContent.Content = new Views.BlanketView();
            UnderviserPanel.Visibility = Visibility.Visible;
        }

        //TODO Ryk til DbTools
        public void OpdaterCounter() {
            var dict = Data.AdminTools.HentAntalEleverPåSkole();
            LyngbyTXT.Text = dict["Lyngby"].ToString();
            BallerupTXT.Text = dict["Ballerup"].ToString();
            FredriksbergTXT.Text = dict["Frederiksberg"].ToString();
        }

        #region Home

        //TODO overvej at fjerne?
        private void btnHome_Click(object sender, RoutedEventArgs e) {
            HomePanel.Visibility = Visibility.Visible;
            UnderviserPanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
            OpdaterCounter();
        }

        #endregion Home

        #region Underviser View

        private void btnUnderviser_Click(object sender, RoutedEventArgs e) {
            //mainContent.Content = new GFElevInterview.Views.maritBlanket();
            mainContent.Content = new Views.BlanketView();
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
            OpdaterCounter();
        }

        #endregion Underviser View

        #region LederView

        //TODO
        private void btnLeder_Click(object sender, RoutedEventArgs e) {
            mainContent.Content = new Views.LoginView(this);
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
            OpdaterCounter();
        }

        public void LederView() {
            mainContent.Content = new Views.LederView();
        }

        #endregion LederView

        private void btnVejled_Click(object sender, RoutedEventArgs e) {
            //mainContent.Content = new Views.VejledningsView();
            UnderviserPanel.Visibility = Visibility.Visible;
            OpdaterCounter();
        }
    }
}