using System.Windows;

namespace GFElevInterview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance = null;  //Singleton

        public MainWindow() {
            InitializeComponent();

            //Singleton setup
            if (Instance == null) {
                Instance = this;
            }

            //Viser `BlanketView` ved opstart.
            UnderviserButton_Click(btnUnderviser, new RoutedEventArgs());
        }

        //TODO ordinær+ og fuldtforløb
        public void OpdaterCounter() {
            var dict = Models.DbTools.Instance.GetAntalEleverPerSkole();
            LyngbyTXT.Text = dict["Lyngby"].ToString();
            BallerupTXT.Text = dict["Ballerup"].ToString();
            FredriksbergTXT.Text = dict["Frederiksberg"].ToString();
        }

        #region Underviser View

        /// <summary>
        /// Viser <see cref="Views.BlanketView"/> viewet.
        /// </summary>
        private void UnderviserButton_Click(object sender, RoutedEventArgs e) {
            //mainContent.Content = new GFElevInterview.Views.maritBlanket();
            mainContent.Content = new Views.BlanketView();
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
            OpdaterCounter();
        }

        #endregion Underviser View

        #region LederView

        /// <summary>
        /// Viser <see cref="Views.LoginView"/> viewet, før <see cref="Views.LederView"/> bliver vist.
        /// </summary>
        private void LederButton_Click(object sender, RoutedEventArgs e) {
            mainContent.Content = new Views.LoginView(this);
            UnderviserPanel.Visibility = Visibility.Visible;
            HomePanel.Visibility = Visibility.Collapsed;
            LederPanel.Visibility = Visibility.Collapsed;
            OpdaterCounter();
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
            OpdaterCounter();
        }
    }
}