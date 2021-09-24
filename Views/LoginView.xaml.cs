using GFElevInterview.Data;
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

        }

        //TODO @Victor Doku 
        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            LoginModel admin = DbTools.Instance.Login.SingleOrDefault(x => x.id == 1);
            if (BC.Verify(txtPassword.Password, admin.password)) {
                CurrentUser.User = admin;
                this.parent.LoginTilLederView();
            }
            else {
                AlertBoxes.OnFailedLoginAttempt();
                txtPassword.Clear();
            }
        }
    }
}