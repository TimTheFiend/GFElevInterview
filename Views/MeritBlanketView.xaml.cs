using GFElevInterview.Data;
using GFElevInterview.Interfaces;
using GFElevInterview.Models;
using System;
using System.Collections.Generic;
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

        public MeritBlanketView(BlanketView parent)
        {
            InitializeComponent();
            this.parent = parent;
            InitialiserBlanket();

            SætEventHandlerComboBox();
        }

        private void SætEventHandlerComboBox()
        {
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
            // Udfyld Blanketten hvis den allerede står som udfyldt i `CurrentElev`.
            //if (CurrentElev.elev.ErUdfyldt) {
            //    //Combobox
            //    cmbDansk.SelectedIndex = (int)CurrentElev.elev.danskNiveau - 1;  // -1 pga `Null` ikke er en del af comboboksen
            //    cmbEngelsk.SelectedIndex = (int)CurrentElev.elev.engelskNiveau - 1;
            //    cmbMatematik.SelectedIndex = (int)CurrentElev.elev.matematikNiveau - 1;
            //    //Checkbox
            //    rbDanskEksamenJa.IsChecked = CurrentElev.elev.danskEksammen;
            //    rbDanskUndervisJa.IsChecked = CurrentElev.elev.danskUndervisning;
            //    rbMatematikEksamenJa.IsChecked = CurrentElev.elev.matematikEksammen;
            //    rbMatematikUndervisJa.IsChecked = CurrentElev.elev.matematikUndervisning;
            //    rbEngelskEksamenJa.IsChecked = CurrentElev.elev.engelskEksammen;
            //    rbEngelskUndervisJa.IsChecked = CurrentElev.elev.engelskUndervisning;
            //}
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

            if(CurrentElev.elev.danskNiveau > FagNiveau.Null)
            {
                UdfyldBlanket.UdfyldComboBox(cmbDansk,(int)CurrentElev.elev.danskNiveau - 1);
                UdfyldBlanket.UdfyldComboBox(cmbEngelsk,(int)CurrentElev.elev.engelskNiveau - 1);
                UdfyldBlanket.UdfyldComboBox(cmbMatematik,(int)CurrentElev.elev.matematikNiveau - 1);

                UdfyldBlanket.UdfyldRadioButton(rbDanskEksamenJa, rbDanskEksamenNej, CurrentElev.elev.danskEksammen);
                UdfyldBlanket.UdfyldRadioButton(rbDanskUndervisJa, rbDanskUndervisNej, CurrentElev.elev.danskUndervisning);
                UdfyldBlanket.UdfyldRadioButton(rbEngelskEksamenJa, rbEngelskEksamenNej, CurrentElev.elev.engelskEksammen);
                UdfyldBlanket.UdfyldRadioButton(rbEngelskUndervisJa, rbEngelskUndervisNej, CurrentElev.elev.engelskUndervisning);
                UdfyldBlanket.UdfyldRadioButton(rbMatematikEksamenJa, rbMatematikEksamenNej, CurrentElev.elev.matematikEksammen);
                UdfyldBlanket.UdfyldRadioButton(rbMatematikUndervisJa, rbMatematikUndervisNej, CurrentElev.elev.matematikUndervisning);
            }
        }

        private void SætKnapper() {
            // Bliver ændret i VisitationsView
            parent.btnFrem.Content = "Frem";

            parent.btnTilbage.IsEnabled = false;
            parent.btnFrem.IsEnabled = true;
        }


        #endregion

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
                //new BlanketUdskrivning().UdskrivningRKV();
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
            Control[] danskControls = new Control[]
            {
                cmbDansk,
                rbDanskEksamenJa,
                rbDanskEksamenNej,
                rbDanskUndervisJa,
                rbDanskUndervisNej
            };
            Control[] engelskControls = new Control[]
            {
                cmbEngelsk,
                rbEngelskEksamenJa,
                rbEngelskEksamenNej,
                rbEngelskUndervisJa,
                rbEngelskUndervisNej
            };
            Control[] matematikControls = new Control[]
            {
                cmbMatematik,
                rbMatematikEksamenJa,
                rbMatematikEksamenNej,
                rbMatematikUndervisJa,
                rbMatematikUndervisNej
            };

            #region Dropdown menu
            if (!ValiderFag(bdrDanskValidation, danskControls))
                erValideret = false;
            if (!ValiderFag(bdrEngelskValidation, engelskControls))
                erValideret = false;
            if (!ValiderFag(bdrMatematikValidation, matematikControls))
                erValideret = false;


            #endregion
            return erValideret;
        }

        //[Combobox, eksamenJa, eksamenNej, UndervisJa, undervisNej]
        private bool ValiderFag(Border border, Control[] control) {
            System.Windows.Media.SolidColorBrush brushTrue = System.Windows.Media.Brushes.Gray;
            System.Windows.Media.SolidColorBrush brushFalse = System.Windows.Media.Brushes.Red;
            bool erValideret = true;

            ComboBox comboBox = control[0] as ComboBox;
            if (comboBox.SelectedIndex == -1) {
                border.BorderBrush = brushFalse;
                erValideret = false;
            }
            else {
                border.BorderBrush = brushTrue;
            }

            for (int i = 0; i < 2; i++) {
                //første omgang = 0 * 2 = 0 + 1 = 1.
                //anden omgang = 1 * 2 = 2 + 1 = 3;
                RadioButton radioJa = control[i * 2 + 1] as RadioButton;
                RadioButton radioNej = control[i * 2 + 2] as RadioButton;

                if (!(bool)radioJa.IsChecked && !(bool)radioNej.IsChecked) {
                    border.BorderBrush = brushFalse;
                    erValideret = false;
                }
            }

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
