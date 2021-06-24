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
            //ComboBox SelectionChanged eventhandler set
            ComboboxDansk.SelectionChanged += ComboboxFagNiveau_SelectionChanged;
            ComboboxEngelsk.SelectionChanged += ComboboxFagNiveau_SelectionChanged;
            ComboboxMatematik.SelectionChanged += ComboboxFagNiveau_SelectionChanged;
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

        private void UdfyldBlanket()
        {
            if(ComboboxDansk == null)
            {
                ComboboxDansk.SelectedItem = CurrentElev.meritBlanket.Dansk.Niveau;
            }
            if(ComboboxEngelsk == null)
            {
                ComboboxEngelsk.SelectedItem = CurrentElev.meritBlanket.Engelsk.Niveau;
            }
            if(ComboboxMatematik == null)
            {
                ComboboxMatematik.SelectedItem = CurrentElev.meritBlanket.Matematik.Niveau;
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

            UdfyldBlanket();
        }
        #endregion

        /// <summary>
        /// Håndterer <see cref="BlanketView"/> knap funktion.
        /// </summary>
        public void Frem() {
            // Tjekker om fag niveau er blevet valgt, da det er det eneste vi med sikkerhed ved at eleven kan have.
            if (IsValidated()) {
                IBlanket newView;
                if (CurrentElev.elev.IsRKV)
                {
                    newView = new EUVView(parent);
                }
                else 
                {
                    newView = new VisitationsView(parent);
                }

                //TODO hvis ikke RKV
                //new BlanketUdskrivning().UdskrivningRKV();
                parent.ChangeView(newView);
            }
        }

        /// <summary>
        /// Håndterer <see cref="BlanketView"/> knap funktion.
        /// </summary>
        public void Tilbage() {
            return;
        }

        /// <summary>
        /// check at der skal have værdi 
        /// </summary>
        /// <returns><c>true</c> hvis valideret; ellers <c>false</c></returns>
        private bool IsValidated() {
            // NYT
            if (ComboboxDansk.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Dansk = new FagModel((bool)DanskEksamenChecked.IsChecked, (bool)DanskUndervisChecked.IsChecked, (FagNiveau)ComboboxDansk.SelectedIndex);
                DanskValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else {
                DanskValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            if (ComboboxEngelsk.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Engelsk = new FagModel((bool)EngelskEksamenChecked.IsChecked, (bool)EngelskUndervisChecked.IsChecked, (FagNiveau)ComboboxEngelsk.SelectedIndex);
                EngelskValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else {
                EngelskValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }

            if (ComboboxMatematik.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Matematik = new FagModel((bool)MatematikEksamenChecked.IsChecked, (bool)MatematikUndervisChecked.IsChecked, (FagNiveau)ComboboxMatematik.SelectedIndex);
                MatematikValidation.BorderBrush = System.Windows.Media.Brushes.Gray;
            }
            else {
                MatematikValidation.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            //// GAMMEL
            if (ComboboxDansk.SelectedIndex >= 0 && ComboboxEngelsk.SelectedIndex >= 0 && ComboboxMatematik.SelectedIndex >= 0) {
                CurrentElev.meritBlanket.Dansk = new FagModel(
                    (bool)DanskEksamenChecked.IsChecked,
                    (bool)DanskUndervisChecked.IsChecked,
                    (FagNiveau)ComboboxDansk.SelectedIndex + 1
                );
                CurrentElev.meritBlanket.Engelsk = new FagModel((bool)EngelskEksamenChecked.IsChecked,
                    (bool)EngelskUndervisChecked.IsChecked,
                    (FagNiveau)ComboboxEngelsk.SelectedIndex + 1
                );
                CurrentElev.meritBlanket.Matematik = new FagModel(
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

        private void ComboboxFagNiveau_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            int selectedIndex = cb.SelectedIndex + 1;

            if (cb == ComboboxDansk)
            {
                CurrentElev.meritBlanket.Dansk.Niveau = (FagNiveau)selectedIndex;
            }
            else if (cb == ComboboxEngelsk)
            {
                CurrentElev.meritBlanket.Engelsk.Niveau = (FagNiveau)selectedIndex;
            }
            else if (cb == ComboboxMatematik)
            {
                CurrentElev.meritBlanket.Matematik.Niveau = (FagNiveau)selectedIndex;
            }

            
           
        }

        
    }
}
