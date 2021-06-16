using System;
using System.Collections.Generic;
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
            var enumItems = Enum.GetNames(typeof(FagNiveau));
            ComboboDansk.ItemsSource = null;
            ComboboDansk.ItemsSource = enumItems;
        }

        //TO DO overfør data fra dansk over til MeritBlanketModel
        private void _IsValidated(object sender, RoutedEventArgs e)
        {
            if(ComboboDansk.SelectedIndex >= 0)
            {
                CurrentElev.meritBlanket.Dansk = new Fag((bool)DanskEksamenChecked.IsChecked,(bool)DanskUndervisChecked.IsChecked,(FagNiveau)ComboboDansk.SelectedIndex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ComboboDansk.SelectedIndex >= 0)
            {
                CurrentElev.meritBlanket.Dansk.Niveau = (FagNiveau)ComboboDansk.SelectedIndex;
            }
        }
    }
}
