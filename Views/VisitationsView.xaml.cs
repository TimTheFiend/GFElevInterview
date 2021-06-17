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

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for WordView.xaml
    /// </summary>
    public partial class VisitationsView : UserControl, IBlanket
    {
        BlanketView parent;

        public VisitationsView(BlanketView parent)
        {
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
                CurrentElev.elev.Fornavn = "Fornavn";
                CurrentElev.elev.Efternavn = "Efternavn";
                CurrentElev.elev.CprNr = 1111931111;
                //ENDTODO
                CurrentElev.visitationsBlanket.UdannelseAdresse = educationAdresseComboBox.Text;
                CurrentElev.visitationsBlanket.Uddannelse = educationComboBox.Text;
                CurrentElev.visitationsBlanket.SPS = (bool)spsSupportJa.IsChecked;
                CurrentElev.visitationsBlanket.EUD = (bool)eudSupportJa.IsChecked;


                UdprintMerit udprint = new UdprintMerit();
                udprint.udprintTilMerit();
                udprint.udprintTilWord();
                parent.UpdateDatabase();
                MessageBox.Show("Dokument gemt! TODO");
            }
        }

        public void Tilbage() {
            parent.ChangeView(new MeritBlanketView(parent));
        }

        //TODO: Valider blanket
        private bool IsValidated() {
            IEnumerable<RadioButton> spsRadioButton = spsSupportGroup.Children.OfType<RadioButton>();
            IEnumerable<RadioButton> eudRadioButton = eudSupportGroup.Children.OfType<RadioButton>();

            if (educationComboBox.SelectedIndex >= 0)
            {
                educationArea.BorderBrush = Brushes.Gray;
            }
            else
            {
                educationArea.BorderBrush = Brushes.Red;
            }
            if (educationAdresseComboBox.SelectedIndex >= 0)
            {
                educationAdresse.BorderBrush = Brushes.Gray;
            }
            else
            {
                educationAdresse.BorderBrush = Brushes.Red;
            }
            if ((bool)spsSupportJa.IsChecked || (bool)spsSupportNej.IsChecked)
            {
                spsSupport.BorderBrush = Brushes.Gray;
            }
            else
            {
                spsSupport.BorderBrush = Brushes.Red;
            }
            if ((bool)eudSupportJa.IsChecked || (bool)eudSupportNej.IsChecked)
            {
                eudSupport.BorderBrush = Brushes.Gray;
            }
            else
            {
                eudSupport.BorderBrush = Brushes.Red;
            }
            if (educationComboBox.SelectedIndex >= 0 && educationAdresseComboBox.SelectedIndex >= 0)
            {


                //nextBlanket = new WordView(); // skal skiftes til RkvView()
                return true;
            }
            //nextBlanket = this;
            return false;
        }

    }
}
