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
        private BlanketView parent;

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

            UdfyldBlanketHvisAlleredeEksisterende();
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
            parent.SkiftBlanket(new MeritBlanketView(parent));
        }

        private bool ErValideret() {
            bool erValideret = true;

            bool erElevEUV1;
            erValideret = InputValidering.ValiderToRadioButtons(rbEuv1Ja, rbEuv1Nej, out erElevEUV1, bdrEuv1Sprg) && erValideret;
            if (erElevEUV1) {
                erValideret = InputValidering.ValiderToRadioButtons(rbEuv1SprgJa, rbEuv1SprgNej, bdrEuv1Sprg) && erValideret;
            }
            else {
                erValideret = InputValidering.ValiderToRadioButtons(rbEuv2Ja, rbEuv2Nej, bdrEuv2) && erValideret;
            }

            erValideret = InputValidering.ValiderComboBox(cmbEducation, bdrEducation) && erValideret;
            erValideret = InputValidering.ValiderComboBox(cmbUddannelse, bdrAdresse) && erValideret;

            erValideret = InputValidering.ValiderToRadioButtons(rbSpsJa, rbSpsNej, bdrSps) && erValideret;
            erValideret = InputValidering.ValiderToRadioButtons(rbEudJa, rbEudNej, bdrEud) && erValideret;

            return erValideret;
        }

        //TODO Udvidelse
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

        private void UdfyldBlanketHvisAlleredeEksisterende() {
            if (CurrentElev.elev.elevType > ElevType.Null) {
                switch (CurrentElev.elev.elevType) {
                    case ElevType.EUV1:
                        //EUV1 == rbEuv1Ja, rbEuv1SprgJa

                        UdfyldEUVRadioButton(rbEuv1Ja, rbEuv1SprgJa);
                        break;

                    case ElevType.EUV2:
                        UdfyldEUVRadioButton(rbEuv1Nej, rbEuv1SprgJa);
                        break;

                    case ElevType.EUV3:
                        UdfyldEUVRadioButton(rbEuv1Nej, rbEuv2Nej);
                        break;
                }

                UdfyldBlanket.UdfyldRadioButton(rbSpsJa, rbSpsNej, CurrentElev.elev.sps);
                UdfyldBlanket.UdfyldRadioButton(rbEudJa, rbEudNej, CurrentElev.elev.eud);

                UdfyldBlanket.UdfyldComboBox(cmbEducation, CurrentElev.elev.uddannelse);
                UdfyldBlanket.UdfyldComboBox(cmbUddannelse, CurrentElev.elev.uddannelseAdresse);
            }
        }

        private void UdfyldEUVRadioButton(RadioButton rbTop, RadioButton rbBund) {
            rbTop.IsChecked = true;
            rbBund.IsChecked = true;
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