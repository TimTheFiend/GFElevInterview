using GFElevInterview.Data;
using GFElevInterview.Interfaces;
using GFElevInterview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for EUVView.xaml
    /// </summary>
    public partial class EUVView : UserControl, IBlanket
    {
        BlanketView parent;

        public EUVView(BlanketView parent) {
            InitializeComponent();
            this.parent = parent;
            InitialiserBlanket();
            //TODO
            rbEuv1Ja.Click += CheckEUVUdvidet;
            rbEuv1Nej.Click += CheckEUVUdvidet;
            rbEuv1Ja.Click += CheckEUVUdvidet;
            rbEuv1Nej.Click += CheckEUVUdvidet;
            cmbEducation.DropDownClosed += Combobox_DropDownClosed;
            cmbUddannelse.DropDownClosed += Combobox_DropDownClosed;
        }

        private void InitialiserBlanket() {
            SætButtons();

            cmbUddannelse.ItemsSource = CurrentElev.elev.ValgAfSkoler();
            if (cmbUddannelse.Items.Count == 1) {
                cmbUddannelse.SelectedIndex = 0;
            }
            expEuv1.IsExpanded = false;
            expEuv1.IsEnabled = false;
            expEuv2.IsExpanded = false;
            expEuv2.IsEnabled = false;
            cmbEducation.ItemsSource = CurrentElev.elev.ValgAfUddannelser();
        }

        private void SætButtons() {
            parent.btnFrem.Content = "Gem";
            parent.btnTilbage.IsEnabled = true;
        }

        public void Frem() {
            if (ErValideret()) {
                SætElevType();
                CurrentElev.elev.uddannelse = cmbEducation.Text.ToString();
                CurrentElev.elev.uddannelseAdresse = cmbUddannelse.Text.ToString();
                CurrentElev.elev.sps = rbSpsJa.IsChecked;
                CurrentElev.elev.eud = rbSpsNej.IsChecked;
                parent.FærdiggørInterview();
            }
        }

        private void SætElevType() {
            ElevType elevType;

            if ((bool)rbEuv1Ja.IsChecked) {
                elevType = ElevType.EUV1;
            }
            else {
                if ((bool)rbEuv2Ja.IsChecked) {
                    elevType = ElevType.EUV2;
                }
                else {
                    elevType = ElevType.EUV3;
                }
            }

            CurrentElev.elev.elevType = elevType;
        }

        public void Tilbage() {
            parent.ChangeView(new MeritBlanketView(parent));
        }


        private bool ErValideret() {
            SolidColorBrush gray = Brushes.Gray;
            SolidColorBrush red = Brushes.Red;

            IEnumerable<RadioButton> spsRadioButton = stackSps.Children.OfType<RadioButton>();
            IEnumerable<RadioButton> eudRadioButton = stackEud.Children.OfType<RadioButton>();
            //Validation
            bool overAllValidated = true;
            //EUV 1
            bool _euv1 = (bool)rbEuv1Ja.IsChecked || (bool)rbEuv1Nej.IsChecked;
            bool _euv1Spg = (bool)rbEuv1SprgJa.IsChecked || (bool)rbEuv1SprgNej.IsChecked;
            //EUV 2
            bool _euv2 = (bool)rbEuv2Ja.IsChecked || (bool)rbEuv2Nej.IsChecked;
            //Education
            bool _educationArea = cmbEducation.SelectedIndex >= 0;
            bool _educationAdresse = cmbUddannelse.SelectedIndex >= 0;
            //Support
            bool _spsSupport = (bool)rbSpsJa.IsChecked || (bool)rbSpsNej.IsChecked;
            bool _eudSupport = (bool)rbEudJa.IsChecked || (bool)rbEudNej.IsChecked;

            //Farv Boxen Grå hvis den er udfyldt eller rød hvis ikke.
            bdrEuv1.BorderBrush = _euv1 ? gray : red;

            bdrEducation.BorderBrush = _educationArea ? gray : red;
            bdrAdresse.BorderBrush = _educationAdresse ? gray : red;

            bdrSps.BorderBrush = _spsSupport ? gray : red;
            bdrEud.BorderBrush = _eudSupport ? gray : red;

            if (!_educationArea || !_educationAdresse || !_euv1 || !_spsSupport || !_eudSupport) {
                overAllValidated = false;
            }
            if (_euv1 && (bool)rbEuv1Ja.IsChecked) {
                if (!_euv1Spg) {
                    bdrEuv1Sprg.BorderBrush = _euv1Spg ? gray : red;
                    overAllValidated = false;
                }
            }
            else if (_euv1) {
                if (!_euv2) {
                    bdrEuv2.BorderBrush = _euv2 ? gray : red;
                    overAllValidated = false;
                }
                //overAllValidated = true;
            }

            //if (_euv1Spg1 && _euv1Spg2 && _euv1Spg3 && _euv1Spg4 && _euv2 && _educationArea && _educationAdresse && _spsSupport && _eudSupport)
            //{
            //    MessageBox.Show("Check");
            //    return true;
            //}
            return overAllValidated;
        }

        private bool ErEUVUdvidet() {
            bool _euv1 = (bool)rbEuv1Ja.IsChecked || !(bool)rbEuv1Nej.IsChecked;
            bool _euv1Spg = (bool)rbEuv1SprgJa.IsChecked || !(bool)rbEuv1SprgNej.IsChecked;
            if (!_euv1) {
                expEuv1.IsExpanded = false;
                expEuv1.IsEnabled = false;
                expEuv2.IsEnabled = true;
                return true;
            }
            if (!_euv1Spg) {
                expEuv1.IsExpanded = false;
                expEuv1.IsEnabled = false;
                expEuv2.IsEnabled = true;
                rbEuv1SprgNej.IsChecked = false;
                rbEuv1Ja.IsChecked = false;
                rbEuv1Nej.IsChecked = true;
                return true;
            }
            expEuv1.IsEnabled = true;
            expEuv1.IsExpanded = true;
            expEuv2.IsEnabled = false;
            return false;
        }
        //Events
        private void Combobox_DropDownClosed(object sender, EventArgs e) {
            parent.scroll.Focus();
        }

        private void CheckEUVUdvidet(object sender, RoutedEventArgs e) {
            expEuv2.IsExpanded = ErEUVUdvidet();
        }
    }
}
