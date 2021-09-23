using GFElevInterview.Data;
using GFElevInterview.Interfaces;
using GFElevInterview.Models;
using System;
using System.Windows;
using System.Windows.Controls;

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
            rbEuv1Ja.Checked += UdfoldEUV_RadioButtonChecked;
            rbEuv1Nej.Checked += UdfoldEUV_RadioButtonChecked;

            cmbEducation.DropDownClosed += Combobox_DropDownClosed;
            cmbUddannelse.DropDownClosed += Combobox_DropDownClosed;

            InitialiserBlanket();
        }

        private void InitialiserBlanket() {
            SætButtons();

            cmbUddannelse.ItemsSource = StandardVaerdier.HentSkoler(CurrentElev.elev.danskNiveau > FagNiveau.F);
            //cmbUddannelse.ItemsSource = CurrentElev.elev.ValgAfSkoler();
            if (cmbUddannelse.Items.Count == 1) {
                cmbUddannelse.SelectedIndex = 0;
            }
            cmbEducation.ItemsSource = StandardVaerdier.HentUddannelser(CurrentElev.elev.erRKV);

            OpdaterExpanders(false, false);
            UdfyldBlanketHvisAlleredeEksisterende();
        }

        private void SætButtons() {
            parent.btnFrem.Content = "Gem";
            parent.btnTilbage.IsEnabled = true;
        }

        private void OpdaterExpanders(bool erEUVJa, bool erEUVNej) {
            expEuv1.IsExpanded = erEUVJa;
            expEuv1.IsEnabled = erEUVJa;

            expEuv2.IsExpanded = erEUVNej;
            expEuv2.IsEnabled = erEUVNej;
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

        public void Tilbage() {
            parent.SkiftBlanket(new MeritBlanketView(parent));
        }

        /// <summary>
        /// Sætter <see cref="CurrentElev.elev"/>s <see cref="ElevType"/> baseret på radioknapper.
        /// </summary>
        private void SætElevType() {
            ElevType elevType = ElevType.Null;
            //Navneændring på rb
            bool rbValider1 = (bool)rbEuv1Ja.IsChecked;
            bool rbValider2 = (bool)rbEuv1SprgJa.IsChecked;
            bool rbValider3 = (bool)rbEuv2Ja.IsChecked;
            //Vi tjekker her at elevtypen bliver udvalgt, udfra de valgte radio knapper.
            if (rbValider1 && rbValider2) {
                elevType = ElevType.EUV1;
            }
            else if ((!rbValider1 && rbValider3) || (rbValider1 && !rbValider2)) {
                elevType = ElevType.EUV2;
            }
            else if (!rbValider1 && !rbValider3) {
                elevType = ElevType.EUV3;
            }
            //Elevtypen bliver indsat i den nuværende elev.
            CurrentElev.elev.elevType = elevType;
        }

        /// <summary>
        /// Validerer om <see cref="ElevType"/> er godkendt ud fra hvilke knapper som er blevet trykket.
        /// Validerer også ComboBox, ved at tjekke ComboBoxen og borderen den er inde under.
        /// </summary>
        /// <returns>erValideret</returns>
        private bool ErValideret() {
            bool erValideret = true;
            bool erElevEUV1;
            //erValideret Tjekker her om radiobuttons er valideret, ved at tjekke knapperne givet til den. hvis den ikke finder
            //den udfyldt så bliver den lyst op med en rød border.
            erValideret = InputValidering.ValiderToRadioButtons(rbEuv1Ja, rbEuv1Nej, out erElevEUV1, bdrEuv1) && erValideret;
            //TODO
            if (erElevEUV1) {
                erValideret = InputValidering.ValiderToRadioButtons(rbEuv1SprgJa, rbEuv1SprgNej, bdrEuv1) && erValideret;
            }
            else {
                erValideret = InputValidering.ValiderToRadioButtons(rbEuv2Ja, rbEuv2Nej, bdrEuv2) && erValideret;
            }
            //Hvis combobox ikke er udfyldt så vil boxen lyse op rødt.
            erValideret = InputValidering.ValiderComboBox(cmbEducation, bdrEducation) && erValideret;
            erValideret = InputValidering.ValiderComboBox(cmbUddannelse, bdrAdresse) && erValideret;

            erValideret = InputValidering.ValiderToRadioButtons(rbSpsJa, rbSpsNej, bdrSps) && erValideret;
            erValideret = InputValidering.ValiderToRadioButtons(rbEudJa, rbEudNej, bdrEud) && erValideret;

            return erValideret;
        }

        /// <summary>
        /// <br>Udfylder blanketten hvis informationerne allerede eksistere.</br>
        /// <br>Den tjekker <see cref="CurrentElev.elev"/> for den valgte elevs informationer.</br>
        /// <br>Knapper bliver sat ved hjælp af <see cref="UdfyldBlanket.UdfyldRadioButton(RadioButton, RadioButton, bool?)"/></br>
        /// og comboBox med <see cref="UdfyldBlanket.UdfyldComboBox(ComboBox, string)"/>
        /// </summary>
        private void UdfyldBlanketHvisAlleredeEksisterende() {
            if (CurrentElev.elev.elevType > ElevType.Null) {
                switch (CurrentElev.elev.elevType) {
                    case ElevType.EUV1:
                        //CheckEUVRadioButton tager de to udvalgte knapper og gør dem true.
                        CheckEUVRadioButton(rbEuv1Ja, rbEuv1SprgJa);
                        break;

                    case ElevType.EUV2:
                        CheckEUVRadioButton(rbEuv1Nej, rbEuv2Ja);
                        break;

                    case ElevType.EUV3:
                        CheckEUVRadioButton(rbEuv1Nej, rbEuv2Nej);
                        break;
                }
                //Vi ufylder her radiobuttons når en udfyldt elev bliver valgt.
                //Den tjekker her om det er Ja eller nej knappen ved at tjekke den nuværende elev.
                UdfyldBlanket.UdfyldRadioButton(rbSpsJa, rbSpsNej, CurrentElev.elev.sps);
                UdfyldBlanket.UdfyldRadioButton(rbEudJa, rbEudNej, CurrentElev.elev.eud);
                //UdfyldComboBox udfylder comboboxen på siden ved at hente informationen fra den nuværende elev.
                UdfyldBlanket.UdfyldComboBox(cmbEducation, CurrentElev.elev.uddannelse);
                UdfyldBlanket.UdfyldComboBox(cmbUddannelse, CurrentElev.elev.uddannelseAdresse);
            }
        }

        private void CheckEUVRadioButton(RadioButton rbTop, RadioButton rbBund) {
            rbTop.IsChecked = true;
            rbBund.IsChecked = true;
        }

        //Events
        private void Combobox_DropDownClosed(object sender, EventArgs e) {
            parent.scroll.Focus();
        }

        private void UdfoldEUV_RadioButtonChecked(object sender, RoutedEventArgs e) {
            OpdaterExpanders((bool)rbEuv1Ja.IsChecked, (bool)rbEuv1Nej.IsChecked);
        }
    }
}