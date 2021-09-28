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

        /// <summary>
        /// Klargører viewet til brug ved opstart.
        /// </summary>
        private void InitialiserBlanket() {
            SætButtons();

            #region Sæt ComboBox værdier
            cmbUddannelse.ItemsSource = Tools.StandardVaerdier.HentSkoler(CurrentElev.elev.DanNiveau > FagNiveau.F);
            
            if (cmbUddannelse.Items.Count == 1) {
                cmbUddannelse.SelectedIndex = 0;
            }
            cmbEducation.ItemsSource = Tools.StandardVaerdier.HentUddannelser(CurrentElev.elev.ErRKV);

            #endregion Sæt ComboBox værdier

            OpdaterExpanders(false, false);
            UdfyldBlanketHvisAlleredeEksisterende();
        }

        /// <summary>
        /// Bliver kaldt på opstart, og ændrer <see cref="parent"/>s knappers udseende.
        /// </summary>
        private void SætButtons() {
            parent.btnFrem.Content = "Gem";
            parent.btnTilbage.IsEnabled = true;
        }

        /// <summary>
        /// Udfolder den passende <see cref="Expander"/> baseret på øverste RadioButton group
        /// </summary>
        private void OpdaterExpanders(bool erEUVJa, bool erEUVNej) {
            //Grunden til at vi modtager to bools, er pga. begge skal være gemt på opstart.
            expEuv1.IsExpanded = erEUVJa;
            expEuv1.IsEnabled = erEUVJa;

            expEuv2.IsExpanded = erEUVNej;
            expEuv2.IsEnabled = erEUVNej;
        }

        #region Frem/Tilbage

        public void Frem() {
            if (ErValideret()) {
                SætElevType();
                CurrentElev.elev.UddLinje = cmbEducation.Text.ToString();
                CurrentElev.elev.UddAdr = cmbUddannelse.Text.ToString();
                CurrentElev.elev.SPS = rbSpsJa.IsChecked;
                CurrentElev.elev.EUD = rbSpsNej.IsChecked;
                parent.FærdiggørInterview();
            }
        }

        public void Tilbage() {
            parent.SkiftBlanket(new MeritBlanketView(parent));
        }

        #endregion Frem/Tilbage

        /// <summary>
        /// Sætter <see cref="CurrentElev.elev"/>s <see cref="EUVType"/> baseret på radioknapper.
        /// </summary>
        private void SætElevType() {
            EUVType elevType = EUVType.Null;
            //Navneændring på rb
            bool rbValider1 = (bool)rbEuv1Ja.IsChecked;
            bool rbValider2 = (bool)rbEuv1SprgJa.IsChecked;
            bool rbValider3 = (bool)rbEuv2Ja.IsChecked;
            //Vi tjekker her at elevtypen bliver udvalgt, udfra de valgte radio knapper.
            if (rbValider1 && rbValider2) {
                elevType = EUVType.EUV1;
            }
            else if ((!rbValider1 && rbValider3) || (rbValider1 && !rbValider2)) {
                elevType = EUVType.EUV2;
            }
            else if (!rbValider1 && !rbValider3) {
                elevType = EUVType.EUV3;
            }
            //Elevtypen bliver indsat i den nuværende elev.
            CurrentElev.elev.ElevType = elevType;
        }

        /// <summary>
        /// Validerer om <see cref="EUVType"/> er godkendt ud fra hvilke knapper som er blevet trykket.
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
            if (CurrentElev.elev.ElevType > EUVType.Null) {
                switch (CurrentElev.elev.ElevType) {
                    case EUVType.EUV1:
                        //CheckEUVRadioButton tager de to udvalgte knapper og gør dem true.
                        CheckEUVRadioButton(rbEuv1Ja, rbEuv1SprgJa);
                        break;

                    case EUVType.EUV2:
                        CheckEUVRadioButton(rbEuv1Nej, rbEuv2Ja);
                        break;

                    case EUVType.EUV3:
                        CheckEUVRadioButton(rbEuv1Nej, rbEuv2Nej);
                        break;
                }
                //Vi ufylder her radiobuttons når en udfyldt elev bliver valgt.
                //Den tjekker her om det er Ja eller nej knappen ved at tjekke den nuværende elev.
                UdfyldBlanket.UdfyldRadioButton(rbSpsJa, rbSpsNej, CurrentElev.elev.SPS);
                UdfyldBlanket.UdfyldRadioButton(rbEudJa, rbEudNej, CurrentElev.elev.EUD);
                //UdfyldComboBox udfylder comboboxen på siden ved at hente informationen fra den nuværende elev.
                UdfyldBlanket.UdfyldComboBox(cmbEducation, CurrentElev.elev.UddLinje);
                UdfyldBlanket.UdfyldComboBox(cmbUddannelse, CurrentElev.elev.UddAdr);
            }
        }

        /// <summary>
        /// Sætter `IsChecked` status på <see cref="RadioButton"/>, hvis blanketten allerede er blevet udfyldt før.
        /// </summary>
        /// <param name="rbTop">Øverste valgte radioknap</param>
        /// <param name="rbBund">Én radioknap fra et af de to andre sæt</param>
        private void CheckEUVRadioButton(RadioButton rbTop, RadioButton rbBund) {
            rbTop.IsChecked = true;
            rbBund.IsChecked = true;
        }

        /// <summary>
        /// Scroller viewet op efter item er blevet valgt.
        /// </summary>
        private void Combobox_DropDownClosed(object sender, EventArgs e) {
            parent.scroll.Focus();
        }

        /// <summary>
        /// Udfolder den passende expander når øverste RadioButton er valgt.
        /// </summary>
        private void UdfoldEUV_RadioButtonChecked(object sender, RoutedEventArgs e) {
            OpdaterExpanders((bool)rbEuv1Ja.IsChecked, (bool)rbEuv1Nej.IsChecked);
        }

        private void rbEuv1Nej_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}