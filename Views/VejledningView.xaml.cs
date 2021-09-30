using GFElevInterview.Tools;
using System.Diagnostics;
using System.Windows.Controls;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for VejledningView.xaml
    /// </summary>
    public partial class VejledningView : UserControl
    {
        public VejledningView() {
            InitializeComponent();
            SetEventHandlers();
        }

        private void SetEventHandlers()
        {
            btnLeder.Click += Brugervejledning_Click;
            btnUnderviser.Click += Brugervejledning_Click;
        }

        //TODO Ren skriv
        private void Brugervejledning_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FilHandler.VisFilIExplorer(
                true, 
                RessourceFil.outputMappeNavn, 
                RessourceFil.brugVejMappe, 
                (sender as Button) == btnLeder ? RessourceFil.brugVejLeder : RessourceFil.brugVejUnderviser
                );
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo(e.Uri.AbsoluteUri);
            info.UseShellExecute = true;

            Process.Start(info);
        }
    }
}