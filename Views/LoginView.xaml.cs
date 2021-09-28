using GFElevInterview.Tools;
using GFElevInterview.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BC = BCrypt.Net.BCrypt;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for LogindView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private MainWindow parent;

        public LoginView(MainWindow parent) {
            InitializeComponent();

            this.parent = parent;
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            txtPassword.Focus();
        }

        /// <summary>
        /// Login Tjekker om vores indtastet password passer over ens med det password(som bliver ukrypteret) fra databasen.
        /// <br/> Brugeren vil herefter forblive logget ind som leder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            LoginModel admin = DbTools.Instance.Login.SingleOrDefault(x => x.id == 1);
            if (BC.Verify(txtPassword.Password, admin.password)) {
                Data.CurrentUser.User = admin;
                this.parent.LoginTilLederView();
            }
            else {
                AlertBoxes.OnFailedLoginAttempt();
                txtPassword.Clear();
            }
        }
    }
}