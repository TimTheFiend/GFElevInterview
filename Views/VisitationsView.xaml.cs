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
        public VisitationsView(BlanketView parent) {
            
            InitializeComponent();
            this.parent = parent;
            InitializeBlanket();
            educationComboBox.DropDownClosed += Combobox_DropDownClosed;
            educationAdresseComboBox.DropDownClosed += Combobox_DropDownClosed;
        }

        private void UdfyldBlanket()

        {
            
            if (!String.IsNullOrEmpty (CurrentElev.elev.Uddannelse))
            {
                educationComboBox.SelectedItem = CurrentElev.elev.Uddannelse;
            }
            if(!String.IsNullOrEmpty(CurrentElev.elev.UdannelseAdresse))
            {
                if(educationAdresseComboBox.Items.Contains(CurrentElev.elev.UdannelseAdresse))
                {
                    educationAdresseComboBox.SelectedItem = CurrentElev.elev.UdannelseAdresse;
                }
            }
           

            switch (CurrentElev.elev.SPS)
            {
                case true:
                    spsSupportJa.IsChecked = true;
                    break;
                case false:
                    spsSupportNej.IsChecked = true;
                    break;
                default:
                    break;
            }


            switch (CurrentElev.elev.EUD)
            {
                case true:
                    eudSupportJa.IsChecked = true;
                    break;
                case false:
                    eudSupportNej.IsChecked = true;
                    break;
                default:
                    break;
            }
        }


        private void InitializeBlanket() {
            
            SetButtons();
            

            educationAdresseComboBox.ItemsSource = CurrentElev.meritBlanket.AvailableSchools();
            if (educationAdresseComboBox.Items.Count == 1) {
                educationAdresseComboBox.SelectedIndex = 0;
            }

            //TODO RKV
            educationComboBox.ItemsSource = CurrentElev.meritBlanket.AvailableEducations();
        }

        

        private void SetButtons() {
            if (true) {
                //TODO Hvis ikke RKV
                parent.btnFrem.Content = "Gem";
            }

            parent.btnTilbage.IsEnabled = true;
        }

        public void Frem() {
            if (IsValidated())
            {

                //TODO Hvis ikke RKV
                //TODO Udprint
                //TODO Få fra Søgning
                UpdateElevAndSave();
               
            }
        }

        private void UpdateElevAndSave()
        {
            //TODO: Få info fra søgefeldt
            //NOTE: Hardcoded

            //ENDTODO
            CurrentElev.elev.UdannelseAdresse = educationAdresseComboBox.Text;
            CurrentElev.elev.Uddannelse = educationComboBox.Text;
            CurrentElev.elev.SPS = (bool)spsSupportJa.IsChecked;
            CurrentElev.elev.EUD = (bool)eudSupportJa.IsChecked;


            UdprintMerit udprint = new UdprintMerit();
            udprint.udprintTilMerit();
            parent.UpdateDatabase();
            //udprint.indPrintTilDataBase();
            MessageBox.Show("Dokument gemt! TODO");
        }

        public void clearRadioButtons()
        {
            if(spsSupportNej.IsChecked == true)
            {
                spsSupportNej.IsChecked = false;
            }
            if(eudSupportNej.IsChecked== true)
            {
                eudSupportNej.IsChecked = false;
            }
            
        }

        public void Tilbage() {
            parent.ChangeView(new MeritBlanketView(parent));
            
        }

        //TODO: Valider blanket
        private bool IsValidated() {
            SolidColorBrush gray = Brushes.Gray;
            SolidColorBrush red = Brushes.Red;

            IEnumerable<RadioButton> spsRadioButton = spsSupportGroup.Children.OfType<RadioButton>();
            IEnumerable<RadioButton> eudRadioButton = eudSupportGroup.Children.OfType<RadioButton>();

            bool _educationArea = educationComboBox.SelectedIndex >= 0;
            bool _educationAdresse = educationAdresseComboBox.SelectedIndex >= 0;
            bool _spsSupport = (bool)spsSupportJa.IsChecked || (bool)spsSupportNej.IsChecked;
            bool _eudSupport = (bool)eudSupportJa.IsChecked || (bool)eudSupportNej.IsChecked;

            //Farv Boxen Grå hvis den er udfyldt eller rød hvis ikke.
            educationArea.BorderBrush = _educationArea ? gray : red;
            educationAdresse.BorderBrush = _educationAdresse ? gray : red;
            spsSupport.BorderBrush = _spsSupport ? gray : red;
            eudSupport.BorderBrush = _eudSupport ? gray : red;


            if (_educationArea && _educationAdresse && _spsSupport && _eudSupport)
            {
                return true;
            }
            return false;
        }
        private void Combobox_DropDownClosed(object sender, EventArgs e)
        {
            parent.scrollview.Focus();
        }

        private void educationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentElev.elev.Uddannelse = (sender as ComboBox).SelectedItem.ToString();
        }

        private void educationAdresseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentElev.elev.UdannelseAdresse = (sender as ComboBox).SelectedItem.ToString();
        }

        
        private void SPSSupport_Checked(object sender, RoutedEventArgs e)
        {
            CurrentElev.elev.SPS = (sender as RadioButton) == spsSupportJa ? true : false;
        }

        private void EUDSupport_Checked(object sender, RoutedEventArgs e)
        {
            CurrentElev.elev.EUD = (sender as RadioButton) == eudSupportJa ? true : false;
            
        }
    }  
}
