using System.Windows;
using System.Windows.Input;

namespace GFElevInterview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance = null;

        public MainWindow() {
            if (instance == null) {
                instance = this;
            }

            InitializeComponent();
            //this.DataContext =;
            //Data.AdminTools.HentAntalEleverPåSkole();

            //new DbTools().TilføjElever();
            OpdaterCounter();
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
        #endregion

        #region Underviser View
        private void btnUnderviser_Click(object sender, RoutedEventArgs e) {
            //mainContent.Content = new GFElevInterview.Views.maritBlanket();
            mainContent.Content = new Views.BlanketView();
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
            OpdaterCounter();
        }
        #endregion

        #region LederView
        //todo
        private void btnLeder_Click(object sender, RoutedEventArgs e) {
            mainContent.Content = new Views.LederView();
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
            OpdaterCounter();
        }
        //todo
        private void signinButton_Click(object sender, RoutedEventArgs e) {
            //GFElevInterview.Views.LederView LederView = new Views.LederView();
            //if (passwordText.Password == "1234")
            //{
            //    LederView.Show();
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

        private void passwordText_KeyDown(object sender, KeyEventArgs e) {
        }
        #endregion
        private void btnVejled_Click(object sender, RoutedEventArgs e)
        {
            //mainContent.Content = new Views.VejledningsView();
            UnderviserPanel.Visibility = Visibility.Visible;
            OpdaterCounter();
        }
    }
}