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
        public int TESTTESTTEST;

        public MeritBlanketView()
        {
            InitializeComponent();
            InitializeComboBox();

            if (CurrentElev.meritBlanket.IsFilled)
            {
                ComboboDansk.SelectedIndex = (int)CurrentElev.meritBlanket.Dansk.Niveau - 1;  // -1 pga `Null` ikke er en del af comboboksen
                ComboboxEngelsk.SelectedIndex = (int)CurrentElev.meritBlanket.Engelsk.Niveau - 1;
                ComboboxMatematik.SelectedIndex = (int)CurrentElev.meritBlanket.Matematik.Niveau - 1;
                //Merit blanketten er fyldt ud.
            }
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

                nextBlanket = new VisitationsView();
                return true;
            }
            nextBlanket = this;
            return false;
        }

        public bool Tilbage(out IBlanket previousBlanket) {
            previousBlanket = null;
            return false;
        }

        private void InitializeComboBox()
        {
            var enumKeysArray = Enum.GetNames(typeof(FagNiveau)).Where(x => x != FagNiveau.Null.ToString());
            ComboboDansk.ItemsSource = enumKeysArray;
            ComboboxEngelsk.ItemsSource = enumKeysArray;
            ComboboxMatematik.ItemsSource = enumKeysArray;
        }

        //TO DO overfør data fra dansk over til MeritBlanketModel
        private void IsValidated()
        {
            if(ComboboDansk.SelectedIndex >= 0 && ComboboxEngelsk.SelectedIndex >= 0 && ComboboxMatematik.SelectedIndex >= 0)
            {
                CurrentElev.meritBlanket.Dansk = new Fag((bool)DanskEksamenChecked.IsChecked,(bool)DanskUndervisChecked.IsChecked,(FagNiveau)ComboboDansk.SelectedIndex);
                CurrentElev.meritBlanket.Engelsk = new Fag((bool)EngelskEksamenChecked.IsChecked, (bool)EngelskUndervisChecked.IsChecked, (FagNiveau)ComboboxEngelsk.SelectedIndex);
                CurrentElev.meritBlanket.Matematik = new Fag((bool)MatematikEksamenChecked.IsChecked, (bool)MatematikUndervisChecked.IsChecked, (FagNiveau)ComboboxMatematik.SelectedIndex);
            }
        }
    }
}
