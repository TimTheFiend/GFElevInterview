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
            //TODO
            rbEuv1Ja.Checked += UdfoldEUV_RadioButtonChecked;
            rbEuv1Nej.Checked += UdfoldEUV_RadioButtonChecked;
            //rbEuv1SprgNej.Click += EUV1_Nej_RadiobuttonSelected;
            //rbEuv1Nej.Click += CheckEUVUdvidet;
            //rbEuv1Ja.Click += CheckEUVUdvidet;
            //rbEuv1Nej.Click += CheckEUVUdvidet;
            cmbEducation.DropDownClosed += Combobox_DropDownClosed;
            cmbUddannelse.DropDownClosed += Combobox_DropDownClosed;
            InitialiserBlanket();
        }

        private void InitialiserBlanket() {
            SætButtons();

            cmbUddannelse.ItemsSource = CurrentElev.elev.ValgAfSkoler();
            if (cmbUddannelse.Items.Count == 1) {
                cmbUddannelse.SelectedIndex = 0;
            }
            cmbEducation.ItemsSource = CurrentElev.elev.ValgAfUddannelser();

            OpdaterExpanders(false, false);
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

        public void Tilbage()
        {
            parent.SkiftBlanket(new MeritBlanketView(parent));
        }

        private void SætElevType() {
            ElevType elevType;
            bool rb1 = (bool)rbEuv1Ja.IsChecked;
            bool rb2 = (bool)rbEuv1SprgJa.IsChecked;
            bool rb3 = (bool)rbEuv2Ja.IsChecked;

            if(rb1 && rb2)
            {
                //EUV 1
                elevType = ElevType.EUV1;
            }
            if((!rb1 && rb3) || (rb1 && !rb2))
            {
                //EUV 2
                elevType = ElevType.EUV2;
            }
            if(!rb1 && !rb3)
            {
                //EUV 3
                elevType = ElevType.EUV3;
            }
            //TODO Max uddyb
            //if ((bool)rbEuv1Ja.IsChecked && (bool)rbEuv1SprgJa.IsChecked) {
            //    elevType = ElevType.EUV1;
            //}
            //if((bool)rbEuv1Ja.IsChecked)
            //else {
            //    if ((bool)rbEuv2Ja.IsChecked) {
            //        elevType = ElevType.EUV2;
            //    }
            //    else {
            //        elevType = ElevType.EUV3;
            //    }
            //}

            CurrentElev.elev.elevType = elevType;
        }

        private bool ErValideret() {
            bool erValideret = true;

            bool erElevEUV1;
            erValideret = InputValidering.ValiderToRadioButtons(rbEuv1Ja, rbEuv1Nej, out erElevEUV1, bdrEuv1) && erValideret;
            if (erElevEUV1) {
                erValideret = InputValidering.ValiderToRadioButtons(rbEuv1SprgJa, rbEuv1SprgNej, bdrEuv1) && erValideret;
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

            //if(elevType == ElevType.EUV1)
            //{
            //    expEuv1.IsExpanded = true;
            //}
            //if(elevType == ElevType.EUV2 || elevType == ElevType.EUV3)
            //{
            //    expEuv2.IsExpanded = false;
            //}

            //return;


            bool _euv1 = (bool)rbEuv1Ja.IsChecked || !(bool)rbEuv1Nej.IsChecked;
            bool _euv1Spg = (bool)rbEuv1SprgJa.IsChecked || !(bool)rbEuv1SprgNej.IsChecked;
            if (!_euv1) {
                expEuv1.IsExpanded = false;
                expEuv1.IsEnabled = false;
                expEuv2.IsEnabled = true;
                expEuv2.IsExpanded = true;
                return true;
            }
            if (!_euv1Spg) {
                expEuv1.IsExpanded = false;
                expEuv1.IsEnabled = false;
                expEuv2.IsEnabled = true;
                rbEuv1SprgNej.IsChecked = false;
                rbEuv1Ja.IsChecked = false;
                rbEuv1Nej.IsChecked = true;
                expEuv2.IsExpanded = true;
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
                        UdfyldEUVRadioButton(rbEuv1Nej, rbEuv2Ja);
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

        private void UdfyldEUVRadioButton(RadioButton rbTop, RadioButton rbBund)
        {
            //TODO
            //OpdaterExpanders(rbTop);

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


        private void UdfoldEUV_RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            //if(sender as RadioButton == rbEuv1Ja)
            //{
            //    OpdaterExpanders(true, false);
            //}
            //else
            //{
            //    OpdaterExpanders(false, true);
            //}
            OpdaterExpanders((bool)rbEuv1Ja.IsChecked, (bool)rbEuv1Nej.IsChecked);
        }

        private void OpdaterExpanders(bool erEUVJa, bool erEUVNej)
        {
            expEuv1.IsExpanded = erEUVJa;
            expEuv1.IsEnabled = erEUVJa;

            expEuv2.IsExpanded = erEUVNej;
            expEuv2.IsEnabled = erEUVNej;
        }
    }
}