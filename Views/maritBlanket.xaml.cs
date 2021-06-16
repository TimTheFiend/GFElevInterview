using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GFElevInterview.Data;
using GFElevInterview.Models;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for maritBlanket.xaml
    /// </summary>
    public partial class maritBlanket : UserControl
    {
        public maritBlanket()
        {
            InitializeComponent();
            InitializeComboBox();

            if (CurrentElev.meritBlanket.IsFilled)
            {
                //Merit blanketten er fyldt ud.
            }
        }
        private void InitializeComboBox()
        {
            var enumKeysArray = Enum.GetNames(typeof(FagNiveau)).Where(x => x != FagNiveau.Null.ToString());
            //ComboboDansk.ItemsSource = null;
            //ComboboDansk.ItemsSource = enumItems;
            ComboboDansk.ItemsSource = enumKeysArray;
            ComboboxEngelsk.ItemsSource = enumKeysArray;
            ComboboxMatematik.ItemsSource = enumKeysArray;
        }

        //TO DO overfør data fra dansk over til MeritBlanketModel
        private void _IsValidated(object sender, RoutedEventArgs e)
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
