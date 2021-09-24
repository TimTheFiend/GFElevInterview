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

        private void SætEventHandlerComboBox() {
            //TODO ind i InitialiserComboBox
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

        //TODO
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

        //TODO
        private void UdfyldBlanketHvisAlleredeEksisterende() {
            if (CurrentElev.elev.danskNiveau > FagNiveau.Null) {
                UdfyldBlanket.UdfyldComboBox(cmbDansk, (int)CurrentElev.elev.danskNiveau - 1);
                UdfyldBlanket.UdfyldComboBox(cmbEngelsk, (int)CurrentElev.elev.engelskNiveau - 1);
                UdfyldBlanket.UdfyldComboBox(cmbMatematik, (int)CurrentElev.elev.matematikNiveau - 1);

                UdfyldBlanket.UdfyldRadioButton(rbDanskEksamenJa, rbDanskEksamenNej, CurrentElev.elev.danskEksamen);
                UdfyldBlanket.UdfyldRadioButton(rbDanskUndervisJa, rbDanskUndervisNej, CurrentElev.elev.danskUndervisning);
                UdfyldBlanket.UdfyldRadioButton(rbEngelskEksamenJa, rbEngelskEksamenNej, CurrentElev.elev.engelskEksamen);
                UdfyldBlanket.UdfyldRadioButton(rbEngelskUndervisJa, rbEngelskUndervisNej, CurrentElev.elev.engelskUndervisning);
                UdfyldBlanket.UdfyldRadioButton(rbMatematikEksamenJa, rbMatematikEksamenNej, CurrentElev.elev.matematikEksamen);
                UdfyldBlanket.UdfyldRadioButton(rbMatematikUndervisJa, rbMatematikUndervisNej, CurrentElev.elev.matematikUndervisning);
            }
        }

        private void SætKnapper() {
            // Bliver ændret i VisitationsView
            parent.btnFrem.Content = "Frem";

            parent.btnTilbage.IsEnabled = false;
            parent.btnFrem.IsEnabled = true;
        }

        #endregion Klargøring

        /// <summary>
        /// Håndterer <see cref="BlanketView"/> knap funktion.
        /// </summary>
        public void Frem() {
            // Tjekker om fag niveau er blevet valgt, da det er det eneste vi med sikkerhed ved at eleven kan have.
            if (ErValideret()) {
                IBlanket newView;
                SætCurrentElevVærdier();
                if (CurrentElev.elev.erRKV) {
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

        //
        private void SætCurrentElevVærdier() {
            CurrentElev.elev.danskNiveau = (FagNiveau)cmbDansk.SelectedIndex + 1;
            CurrentElev.elev.engelskNiveau = (FagNiveau)cmbEngelsk.SelectedIndex + 1;
            CurrentElev.elev.matematikNiveau = (FagNiveau)cmbMatematik.SelectedIndex + 1;

            //NOTE Flag
            CurrentElev.elev.SætMeritStatus(Merit.DanskEksamen, (bool)rbDanskEksamenJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.DanskUndervisning, (bool)rbDanskUndervisJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.EngelskEksamen, (bool)rbEngelskEksamenJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.EngelskUndervisning, (bool)rbEngelskUndervisJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.MatematikEksamen, (bool)rbMatematikEksamenJa.IsChecked);
            CurrentElev.elev.SætMeritStatus(Merit.MatematikUndervisning, (bool)rbMatematikUndervisJa.IsChecked);
        }

        /// <summary>
        /// check at der skal have værdi
        /// </summary>
        /// <returns><c>true</c> hvis valideret; ellers <c>false</c></returns>
        private bool ErValideret() {
            bool erValideret = true;

            erValideret = InputValidering.ValiderMerit(rbDanskEksamenJa, rbDanskEksamenNej, rbDanskUndervisJa, rbDanskUndervisNej, cmbDansk, bdrDanskValidation) && erValideret;
            erValideret = InputValidering.ValiderMerit(rbMatematikEksamenJa, rbMatematikEksamenNej, rbMatematikUndervisJa, rbMatematikUndervisNej, cmbMatematik, bdrMatematikValidation) && erValideret;
            erValideret = InputValidering.ValiderMerit(rbEngelskEksamenJa, rbEngelskEksamenNej, rbEngelskUndervisJa, rbEngelskUndervisNej, cmbEngelsk, bdrEngelskValidation) && erValideret;

            return erValideret;
        }

        //Events
        private void Combobox_DropDownClosed(object sender, EventArgs e) {
            //Change the focus to scrollview in BlanketView.
            parent.scroll.Focus();
        }

        // TODO
        ///Mangel på obejct refernce i CurrentElev.elev.niveau
        ///CurrentElev.elev virker til at have mangel på informationer, hvilket kunne skyldes
        private void ComboboxFagNiveau_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cb = sender as ComboBox;
            int selectedIndex = cb.SelectedIndex + 1;

            if (cb == cmbDansk) {
                CurrentElev.elev.danskNiveau = (FagNiveau)selectedIndex;
            }
            else if (cb == cmbEngelsk) {
                CurrentElev.elev.engelskNiveau = (FagNiveau)selectedIndex;
            }
            else if (cb == cmbMatematik) {
                CurrentElev.elev.matematikNiveau = (FagNiveau)selectedIndex;
            }
        }
    }
}