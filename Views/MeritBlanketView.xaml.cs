using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GFElevInterview.Data;
using GFElevInterview.Interfaces;
using GFElevInterview.Models;

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
            InitializeBlanket();
            //Combobox gets it dropdownclosed from the method called "Combobox_DropDownClosed".
            ComboboxMatematik.DropDownClosed += Combobox_DropDownClosed;
            ComboboxDansk.DropDownClosed += Combobox_DropDownClosed;
            ComboboxEngelsk.DropDownClosed += Combobox_DropDownClosed;
        }

        #region Klargøring
        private void InitializeBlanket() {
            InitializeComboBox();
            SetButtons();
            // Udfyld Blanketten hvis den allerede står som udfyldt i `CurrentElev`.
            if (CurrentElev.meritBlanket.IsFilled) {
                //Combobox
                ComboboxDansk.SelectedIndex = (int)CurrentElev.meritBlanket.Dansk.Niveau - 1;  // -1 pga `Null` ikke er en del af comboboksen
                ComboboxEngelsk.SelectedIndex = (int)CurrentElev.meritBlanket.Engelsk.Niveau - 1;
                ComboboxMatematik.SelectedIndex = (int)CurrentElev.meritBlanket.Matematik.Niveau - 1;
                //Checkbox
                DanskEksamenChecked.IsChecked = CurrentElev.meritBlanket.Dansk.Eksamen;
                DanskUndervisChecked.IsChecked = CurrentElev.meritBlanket.Dansk.Undervisning;
                MatematikEksamenChecked.IsChecked = CurrentElev.meritBlanket.Matematik.Eksamen;
                MatematikUndervisChecked.IsChecked = CurrentElev.meritBlanket.Matematik.Undervisning;
                EngelskEksamenChecked.IsChecked = CurrentElev.meritBlanket.Engelsk.Eksamen;
                EngelskUndervisChecked.IsChecked = CurrentElev.meritBlanket.Engelsk.Undervisning;

            }
        }

        private void SetButtons() {
            // Bliver ændret i VisitationsView
            parent.btnFrem.Content = "Frem";

            parent.btnTilbage.IsEnabled = false;
            parent.btnFrem.IsEnabled = true;
        }

        /// <summary>
        /// Fylder ComboBoxene op med karaktere.
        /// </summary>
        private void InitializeComboBox() {
            var enumKeysArray = Enum.GetNames(typeof(FagNiveau)).Where(x => x != FagNiveau.Null.ToString());
            ComboboxDansk.ItemsSource = enumKeysArray;
            ComboboxEngelsk.ItemsSource = enumKeysArray;
            ComboboxMatematik.ItemsSource = enumKeysArray;
        }
        #endregion

        /// <summary>
        /// Håndterer <see cref="BlanketView"/> knap funktion.
        /// </summary>
        public void Frem() {
            // Tjekker om fag niveau er blevet valgt, da det er det eneste vi med sikkerhed ved at eleven kan have.
            if (IsValidated()) {
                //TODO hvis ikke RKV
                new BlanketUdskrivning().UdskrivningRKV();
                parent.ChangeView(new VisitationsView(parent));
            }
        }

        /// <summary>
        /// Håndterer <see cref="BlanketView"/> knap funktion.
        /// </summary>
        public void Tilbage() {
            return;
        }

        //TODO: Overfør info fra checkbox
        //NOTE: Kluntet

        private bool IsValidated() {
            // NYT
            if (ComboboxDansk.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Dansk = new Fag((bool)DanskEksamenChecked.IsChecked, (bool)DanskUndervisChecked.IsChecked, (FagNiveau)ComboboxDansk.SelectedIndex);
                DanskValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else {
                DanskValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            if (ComboboxEngelsk.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Engelsk = new Fag((bool)EngelskEksamenChecked.IsChecked, (bool)EngelskUndervisChecked.IsChecked, (FagNiveau)ComboboxEngelsk.SelectedIndex);
                EngelskValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else {
                EngelskValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }

            if (ComboboxMatematik.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Matematik = new Fag((bool)MatematikEksamenChecked.IsChecked, (bool)MatematikUndervisChecked.IsChecked, (FagNiveau)ComboboxMatematik.SelectedIndex);
                MatematikValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else {
                MatematikValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            //// GAMMEL
            if (ComboboxDansk.SelectedIndex >= 0 && ComboboxEngelsk.SelectedIndex >= 0 && ComboboxMatematik.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Dansk = new Fag(
                    (bool)DanskEksamenChecked.IsChecked,
                    (bool)DanskUndervisChecked.IsChecked,
                    (FagNiveau)ComboboxDansk.SelectedIndex + 1
                );
                CurrentElev.meritBlanket.Engelsk = new Fag((bool)EngelskEksamenChecked.IsChecked,
                    (bool)EngelskUndervisChecked.IsChecked,
                    (FagNiveau)ComboboxEngelsk.SelectedIndex + 1
                );
                CurrentElev.meritBlanket.Matematik = new Fag(
                    (bool)MatematikEksamenChecked.IsChecked,
                    (bool)MatematikUndervisChecked.IsChecked,
                    (FagNiveau)ComboboxMatematik.SelectedIndex + 1
                );

                return true;
            }
            return false;
        }


        private void Combobox_DropDownClosed(object sender, EventArgs e)
        {
            //Change the focus to scrollview in BlanketView.
            parent.scrollview.Focus();
        }
    }
}
