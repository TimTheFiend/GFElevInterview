using GFElevInterview.Data;
using GFElevInterview.Tools;
using GFElevInterview.Interfaces;
using GFElevInterview.Models;
using System;
using System.Linq;
using System.Windows.Controls;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for maritBlanket.xaml
    /// </summary>
    public partial class MeritBlanketView : UserControl, IBlanket
    {
        public BlanketView parent;

        public MeritBlanketView(BlanketView parent) {
            InitializeComponent();

            this.parent = parent;
            InitialiserBlanket();

            SætEventHandlerComboBox();
        }

        /// <summary>
        /// Sætter EventHandlers til ComboBoxes
        /// </summary>
        private void SætEventHandlerComboBox() {
            //Combobox gets it dropdownclosed from the method called "Combobox_DropDownClosed".
            cmbMatematik.DropDownClosed += Combobox_DropDownClosed;
            cmbDansk.DropDownClosed += Combobox_DropDownClosed;
            cmbEngelsk.DropDownClosed += Combobox_DropDownClosed;

            //ComboBox SelectionChanged eventhandler set
            cmbDansk.SelectionChanged += ComboboxFagNiveau_SelectionChanged;
            cmbEngelsk.SelectionChanged += ComboboxFagNiveau_SelectionChanged;
            cmbMatematik.SelectionChanged += ComboboxFagNiveau_SelectionChanged;
        }

        #region Klargøring


        /// <summary>
        /// Klargører viewet til brug ved opstart.
        /// </summary>
        private void InitialiserBlanket() {
        InitialiserComboBox();
            SætKnapper();
        }

        /// <summary>
        /// Fylder ComboBoxene op med karaktere.
        /// </summary>
        private void InitialiserComboBox() {
            var fagNiveauArray = Enum.GetNames(typeof(FagNiveau)).Where(x => x != FagNiveau.Null.ToString());
            cmbDansk.ItemsSource = fagNiveauArray;
            cmbEngelsk.ItemsSource = fagNiveauArray;
            cmbMatematik.ItemsSource = fagNiveauArray;

            UdfyldBlanketHvisAlleredeEksisterende();
        }

        /// <summary>
        /// <br>Udfylder blanketten hvis informationerne allerede eksistere.</br>
        /// <br>Den tjekker <see cref="CurrentElev.elev"/> for den valgte elevs informationer.</br>
        /// <br>Knapper bliver sat ved hjælp af <see cref="UdfyldBlanket.UdfyldRadioButton(RadioButton, RadioButton, bool?)"/></br>
        /// og comboBox med <see cref="UdfyldBlanket.UdfyldComboBox(ComboBox, string)"/>
        /// </summary>
        private void UdfyldBlanketHvisAlleredeEksisterende() {
            if (CurrentElev.elev.DanNiveau > FagNiveau.Null) {
                UdfyldBlanket.UdfyldComboBox(cmbDansk, (int)CurrentElev.elev.DanNiveau - 1);
                UdfyldBlanket.UdfyldComboBox(cmbEngelsk, (int)CurrentElev.elev.EngNiveau - 1);
                UdfyldBlanket.UdfyldComboBox(cmbMatematik, (int)CurrentElev.elev.MatNiveau - 1);

                UdfyldBlanket.UdfyldRadioButton(rbDanskEksamenJa, rbDanskEksamenNej, CurrentElev.elev.DanEksamen);
                UdfyldBlanket.UdfyldRadioButton(rbDanskUndervisJa, rbDanskUndervisNej, CurrentElev.elev.DanUndervisning);
                UdfyldBlanket.UdfyldRadioButton(rbEngelskEksamenJa, rbEngelskEksamenNej, CurrentElev.elev.EngEksamen);
                UdfyldBlanket.UdfyldRadioButton(rbEngelskUndervisJa, rbEngelskUndervisNej, CurrentElev.elev.EngUndervisning);
                UdfyldBlanket.UdfyldRadioButton(rbMatematikEksamenJa, rbMatematikEksamenNej, CurrentElev.elev.MatEksamen);
                UdfyldBlanket.UdfyldRadioButton(rbMatematikUndervisJa, rbMatematikUndervisNej, CurrentElev.elev.MatUndervisning);
            }
        }

        /// <summary>
        /// Bliver kaldt på opstart, og ændrer <see cref="parent"/>s knappers udseende.
        /// </summary>
        private void SætKnapper() {
            // Bliver ændret i VisitationsView
            parent.btnFrem.Content = "Frem";

            parent.btnTilbage.IsEnabled = false;
            parent.btnFrem.IsEnabled = true;
        }

        #endregion Klargøring

        #region Frem/Tilbage

        /// <summary>
        /// Håndterer <see cref="BlanketView"/> knap funktion.
        /// </summary>
        public void Frem() {
            // Tjekker om fag niveau er blevet valgt, da det er det eneste vi med sikkerhed ved at eleven kan have.
            if (ErValideret()) {
                IBlanket newView;
                SætCurrentElevVærdier();
                if (CurrentElev.elev.ErRKV) {
                    newView = new EUVView(parent);
                }
                else {
                    newView = new VisitationsView(parent);
                }
                //TODO hvis ikke RKV
                if (newView == null) {
                    AlertBoxes.OnUnlikelyError();
                }
                parent.SkiftBlanket(newView);
            }
        }

        /// <summary>
        /// Håndterer <see cref="BlanketView"/> knap funktion.
        /// </summary>
        public void Tilbage() {
            return;
        }

        #endregion Frem/Tilbage

        /// <summary>
        /// Sætter blanket værdier ind i <see cref="CurrentElev.elev"/>.
        /// </summary>
        private void SætCurrentElevVærdier() {
            CurrentElev.elev.DanNiveau = (FagNiveau)cmbDansk.SelectedIndex + 1;
            CurrentElev.elev.EngNiveau = (FagNiveau)cmbEngelsk.SelectedIndex + 1;
            CurrentElev.elev.MatNiveau = (FagNiveau)cmbMatematik.SelectedIndex + 1;

            //NOTE Flag
            CurrentElev.elev.SætMeritStatus(Merit.DanskEksamen, (bool)rbDanskEksamenJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.DanskUndervisning, (bool)rbDanskUndervisJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.EngelskEksamen, (bool)rbEngelskEksamenJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.EngelskUndervisning, (bool)rbEngelskUndervisJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.MatematikEksamen, (bool)rbMatematikEksamenJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.MatematikUndervisning, (bool)rbMatematikUndervisJa.IsChecked);
        }

        /// <summary>
        /// Tjekker om blanketten er blevet udfyldt, og highlighter border ved mangler.
        /// </summary>
        /// <returns><c>true</c> hvis valideret; ellers <c>false</c></returns>
        private bool ErValideret() {
            bool erValideret = true;

            erValideret = InputValidering.ValiderMerit(rbDanskEksamenJa, rbDanskEksamenNej, rbDanskUndervisJa, rbDanskUndervisNej, cmbDansk, bdrDanskValidation) && erValideret;
            erValideret = InputValidering.ValiderMerit(rbMatematikEksamenJa, rbMatematikEksamenNej, rbMatematikUndervisJa, rbMatematikUndervisNej, cmbMatematik, bdrMatematikValidation) && erValideret;
            erValideret = InputValidering.ValiderMerit(rbEngelskEksamenJa, rbEngelskEksamenNej, rbEngelskUndervisJa, rbEngelskUndervisNej, cmbEngelsk, bdrEngelskValidation) && erValideret;

            return erValideret;
        }

        /// <summary>
        /// Scroller viewet op efter item er blevet valgt.
        /// </summary>
        private void Combobox_DropDownClosed(object sender, EventArgs e) {
            parent.scroll.Focus();
        }


        /// <summary>
        /// Sætter valgte ComboBox item til <see cref="CurrentElev.elev"/>.
        /// </summary>
        private void ComboboxFagNiveau_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cb = sender as ComboBox;
            int selectedIndex = cb.SelectedIndex + 1;

            if (cb == cmbDansk) {
                CurrentElev.elev.DanNiveau = (FagNiveau)selectedIndex;
            }
            else if (cb == cmbEngelsk) {
                CurrentElev.elev.EngNiveau = (FagNiveau)selectedIndex;
            }
            else if (cb == cmbMatematik) {
                CurrentElev.elev.MatNiveau = (FagNiveau)selectedIndex;
            }
        }
    }
}