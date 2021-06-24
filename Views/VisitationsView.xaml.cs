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
        /* Fields */
        BlanketView parent;

        /* Constructor */
        public VisitationsView(BlanketView parent) {
            InitializeComponent();
            this.parent = parent;
            InitializeBlanket();
            educationComboBox.DropDownClosed += Combobox_DropDownClosed;
            educationAdresseComboBox.DropDownClosed += Combobox_DropDownClosed;
        }


        #region Klargøringsmetoder
        private void InitializeBlanket() {
            SetButtons();
            SetComboBoxes();

            UdfyldBlanket();
        }

        private void SetComboBoxes() {
            educationAdresseComboBox.ItemsSource = CurrentElev.meritBlanket.AvailableSchools();

            ///Får programmet til at crashe da SelectedItem = null (???)
            //if (educationAdresseComboBox.Items.Count == 1) {
            //    educationAdresseComboBox.SelectedIndex = 0;
            //}

            educationComboBox.ItemsSource = CurrentElev.meritBlanket.AvailableEducations();
        }

        private void SetButtons() {
            parent.btnFrem.Content = "Gem";
            parent.btnTilbage.IsEnabled = true;
        }

        /// <summary>
        /// udfyldning af visitationsView
        /// </summary>
        private void UdfyldBlanket()
        {
            if (!String.IsNullOrEmpty (CurrentElev.elev.Uddannelse))
            {
                educationComboBox.SelectedItem = CurrentElev.elev.Uddannelse;
            }

            if (!String.IsNullOrEmpty(CurrentElev.elev.UdannelseAdresse)) {
                //Hvis der er ændret i Dansk karakter, så vil hverken Lyngby eller Frederiksberg blive vist.
                if (educationAdresseComboBox.Items.Contains(CurrentElev.elev.UdannelseAdresse)) {
                    CurrentElev.elev.UdannelseAdresse = null;
                    educationAdresseComboBox.SelectedItem = CurrentElev.elev.UdannelseAdresse;
                }
            }


            switch (CurrentElev.elev.SPS) {
                case true:
                    spsSupportJa.IsChecked = true;
                    break;
                case false:
                    spsSupportNej.IsChecked = true;
                    break;
                default:
                    break;
            }

            switch (CurrentElev.elev.EUD) {
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
        #endregion

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
            //NOTE: Bliver sat før vi overhovedet kommer hertil

            CurrentElev.elev.UdannelseAdresse = educationAdresseComboBox.Text;
            CurrentElev.elev.Uddannelse = educationComboBox.Text;
            CurrentElev.elev.SPS = (bool)spsSupportJa.IsChecked;
            CurrentElev.elev.EUD = (bool)eudSupportJa.IsChecked;
            
            parent.CompleteCurrentInterview();
        }

        /// <summary>Ændr <see cref="BlanketView"/>s <see cref="ContentControl"/> til <see cref="MeritBlanketView"/></summary>
        public void Tilbage() {
            parent.ChangeView(new MeritBlanketView(parent));
        }

        /// <summary>
        /// Bestemmer om siden er valideret og klar til afslutning, og highlighter felter der mangler.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is validated; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidated() {
            SolidColorBrush gray = Brushes.Gray;
            SolidColorBrush red = Brushes.Red;

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

        #region Combobox/Radiobutton Eventhandlers
        #region Combobox
        /// <summary>Fjerner fokus fra combobox når den folder sammen.</summary>
        private void Combobox_DropDownClosed(object sender, EventArgs e) {
            parent.scrollview.Focus();
        }

        /// <summary>Sætter <see cref="CurrentElev.elev"/> værdi på valg fra <see cref="ComboBox"/></summary>
        private void educationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            CurrentElev.elev.Uddannelse = (sender as ComboBox).SelectedItem.ToString();
        }

        /// <summary>Sætter <see cref="CurrentElev.elev"/> værdi på valg fra <see cref="ComboBox"/></summary>
        private void educationAdresseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            CurrentElev.elev.UdannelseAdresse = (sender as ComboBox).SelectedItem.ToString();
        }
        #endregion Combobox

        #region RadioButton setters
        private void SPSSupport_Checked(object sender, RoutedEventArgs e) {

            CurrentElev.elev.SPS = (sender as RadioButton) == spsSupportJa ? true : false;
        }

        private void EUDSupport_Checked(object sender, RoutedEventArgs e) {
            CurrentElev.elev.EUD = (sender as RadioButton) == eudSupportJa ? true : false;
        }
        #endregion Radiobutton setters
        #endregion
    }
}
