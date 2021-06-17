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

        public bool Frem(out IBlanket nextBlanket) {

            if(ComboboDansk.SelectedIndex >=0)
            {
                CurrentElev.meritBlanket.Dansk = new Fag((bool)DanskEksamenChecked.IsChecked, (bool)DanskUndervisChecked.IsChecked, (FagNiveau)ComboboDansk.SelectedIndex);
                DanskValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else
            {
                DanskValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            if(ComboboxEngelsk.SelectedIndex>=0)
            {
                CurrentElev.meritBlanket.Engelsk = new Fag((bool)EngelskEksamenChecked.IsChecked, (bool)EngelskUndervisChecked.IsChecked, (FagNiveau)ComboboxEngelsk.SelectedIndex);
                EngelskValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else
            {
                EngelskValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }

            if(ComboboxMatematik.SelectedIndex >=0)
            {
                CurrentElev.meritBlanket.Matematik = new Fag((bool)MatematikEksamenChecked.IsChecked, (bool)MatematikUndervisChecked.IsChecked, (FagNiveau)ComboboxMatematik.SelectedIndex);
                MatematikValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else
            {
                MatematikValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }

            if (ComboboDansk.SelectedIndex >= 0 && ComboboxEngelsk.SelectedIndex >= 0 && ComboboxMatematik.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Dansk = new Fag((bool)DanskEksamenChecked.IsChecked, (bool)DanskUndervisChecked.IsChecked, (FagNiveau)ComboboDansk.SelectedIndex);
                CurrentElev.meritBlanket.Engelsk = new Fag((bool)EngelskEksamenChecked.IsChecked, (bool)EngelskUndervisChecked.IsChecked, (FagNiveau)ComboboxEngelsk.SelectedIndex);
                CurrentElev.meritBlanket.Matematik = new Fag((bool)MatematikEksamenChecked.IsChecked, (bool)MatematikUndervisChecked.IsChecked, (FagNiveau)ComboboxMatematik.SelectedIndex);

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
