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

        public MeritBlanketView(BlanketView parent)
        {
            InitializeComponent();
            this.parent = parent;
            InitializeMerit();

        }

        #region Klargøringsmetoder
        private void InitializeMerit()
        {
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

        public void Frem() {
            // Tjekker om fag niveau er blevet valgt, da det er det eneste vi med sikkerhed ved at eleven kan have.
            if (IsValidated()) {
                //TODO hvis ikke RKV
                parent.ChangeView(new VisitationsView(parent));
            }
        }

        public void Tilbage() {

        }



        //TODO: Overfør info fra checkbox
        private bool IsValidated()
        {
            if (ComboboxDansk.SelectedIndex >= 0 && ComboboxEngelsk.SelectedIndex >= 0 && ComboboxMatematik.SelectedIndex >= 0)
            {
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
    }
}
