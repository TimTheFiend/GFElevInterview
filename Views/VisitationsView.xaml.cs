using GFElevInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GFElevInterview.Data;


namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for WordView.xaml
    /// </summary>
    public partial class VisitationsView : UserControl, IBlanket
    {
        BlanketView parent;

        public VisitationsView(BlanketView parent)
        {
            InitializeComponent();

            this.parent = parent;
            InitializeBlanket();
        }

        private void InitializeBlanket() {
            SetButtons();

            educationAdresseComboBox.ItemsSource = CurrentElev.meritBlanket.AvailableSchools();

            //TODO
            educationComboBox.ItemsSource = new List<string>() {
                "IT-supporter",
                "Programmør",
                "Infrastruktur",
                "Ved ikke"
            };
        }

        private void SetButtons() {
            if (true) {
                //TODO Hvis ikke RKV
                parent.btnFrem.Content = "Gem";
            }

            parent.btnTilbage.IsEnabled = true;
        }

        public void Frem() {
            if (true) {
                //TODO Hvis ikke RKV
                parent.mainContent.Content = null;
            }
        }

        public void Tilbage() {
            parent.ChangeView(new MeritBlanketView(parent));
        }

        //TODO: Valider blanket
        private bool IsValidated() {
            if (true) {
                return true;
            }
            return false;
        }

    }
}
