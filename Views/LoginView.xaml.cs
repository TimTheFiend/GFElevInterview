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
        private static readonly string _password = "root";
        private MainWindow parent;

        public LoginView(MainWindow parent) {
            InitializeComponent();

            this.parent = parent;
        }

        //Basic login check
        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            if (BC.Verify(txtPassword.Password, DbTools.Instance.Login.SingleOrDefault(x => x.id == 1).password)) {
                this.parent.LoginTilLederView();
            }
            else {
                AlertBoxes.OnFailedLoginAttempt();
                txtPassword.Clear();
            }
        }
    }
}