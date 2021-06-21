using GFElevInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GFElevInterview.Data;
using System.Linq;
using System.Threading;


namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for WordView.xaml
    /// </summary>
    public partial class VisitationsView : UserControl, IBlanket
    {
        BlanketView parent;

        public VisitationsView(BlanketView parent) {
            InitializeComponent();

            this.parent = parent;
            InitializeBlanket();
        }

        private void InitializeBlanket() {
            SetButtons();

            educationAdresseComboBox.ItemsSource = CurrentElev.meritBlanket.AvailableSchools();
            if (educationAdresseComboBox.Items.Count == 1) {

            }

            //TODO RKV
            educationComboBox.ItemsSource = new List<string>() {
                "IT-supporter",
                "Programmør",
                "Infrastruktur",
                "Ved ikke"
            };
        }

        private void SetButtons() {
            if (true) {
                //TODO Hvis ikke RKV
                parent.btnFrem.Content = "Gem";
            }

            parent.btnTilbage.IsEnabled = true;
        }

        public void Frem() {
            if (IsValidated()) {
                //TODO Hvis ikke RKV
                //TODO Udprint
                //TODO Få fra Søgning
                UpdateElevAndSave();
            }
        }

        private void UpdateElevAndSave() {
            //TODO: Få info fra søgefeldt
            //NOTE: Hardcoded

            //ENDTODO
            CurrentElev.elev.UdannelseAdresse = educationAdresseComboBox.Text;
            CurrentElev.elev.Uddannelse = educationComboBox.Text;
            CurrentElev.elev.SPS = (bool)spsSupportJa.IsChecked;
            CurrentElev.elev.EUD = (bool)eudSupportJa.IsChecked;

            //Laver en thread som kalder på `UdprintTilMerit`, som sørger for at programmet ikke hænger som om det er crashet.
            Thread thread = new Thread(new ThreadStart(new UdprintMerit().UdprintTilMerit));
            thread.Start();

            parent.UpdateDatabase();
            //udprint.indPrintTilDataBase();
            MessageBox.Show("Dokument gemt! TODO");
        }

        public void Tilbage() {
            parent.ChangeView(new MeritBlanketView(parent));
        }

        //TODO: Valider blanket
        private bool IsValidated() {
            bool _educationArea = educationComboBox.SelectedIndex >= 0;
            bool _educationAdresse = educationAdresseComboBox.SelectedIndex >= 0;
            bool _spsSupport = (bool)spsSupportJa.IsChecked || (bool)spsSupportNej.IsChecked;
            bool _eudSupport = (bool)eudSupportJa.IsChecked || (bool)eudSupportNej.IsChecked;
            
            #region Farve + Highlight
            SolidColorBrush gray = Brushes.Gray;
            SolidColorBrush red = Brushes.Red;
            educationArea.BorderBrush = _educationArea ? gray : red;
            educationAdresse.BorderBrush = _educationAdresse ? gray : red;
            spsSupport.BorderBrush = _spsSupport ? gray : red;
            eudSupport.BorderBrush = _eudSupport ? gray : red;
            #endregion

            if (_educationArea && _educationAdresse && _spsSupport && _eudSupport) {
                return true;
            }
            return false;
        }

    }
}
