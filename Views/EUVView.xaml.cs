using GFElevInterview.Data;
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

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for EUVView.xaml
    /// </summary>
    public partial class EUVView : UserControl, IBlanket
    {
        BlanketView parent;

        public EUVView(BlanketView parent)
        {
            InitializeComponent();
            this.parent = parent;
            InitializeBlanket();
            EUV2Expand.IsExpanded = false;
            EUV1JA.Click += CheckEUVExpand;
            EUV1NEJ.Click += CheckEUVExpand;
        }

        private void InitializeBlanket()
        {
            SetButtons();

            prioritetbox.ItemsSource = CurrentElev.meritBlanket.AvailableSchools();
            if(prioritetbox.Items.Count == 1)
            {
                prioritetbox.SelectedIndex = 0;
            }

            educationComboBox.ItemsSource = CurrentElev.meritBlanket.AvailableEducations();
        }

        private void SetButtons()
        {
            if (true)
            {
                //TODO Hvis ikke RKV
                parent.btnFrem.Content = "Gem";
            }

            parent.btnTilbage.IsEnabled = true;
        }

        private void ExpandEUV()
        {
            IsEUVExpanded();
        }
        private void UpdateElevAndSave()
        {
            //TODO: Få info fra søgefeldt
            //NOTE: Hardcoded

            //ENDTODO
            CurrentElev.elev.Uddannelse = educationComboBox.Text;
            CurrentElev.elev.SPS = (bool)EUV2SPSJA.IsChecked;
            CurrentElev.elev.EUD = (bool)EUV2EUDJA.IsChecked;


            UdprintMerit udprint = new UdprintMerit();
            udprint.udprintTilMerit();
            parent.UpdateDatabase();
            //udprint.indPrintTilDataBase();
            MessageBox.Show("Dokument gemt! TODO");
        }

        public void Frem()
        {
            throw new NotImplementedException();
        }

        public void Tilbage()
        {
            parent.ChangeView(new MeritBlanketView(parent));
        }

        private bool IsEUVExpanded()
        {
            bool _euv1 = (bool)EUV1JA.IsChecked || !(bool)EUV1NEJ.IsChecked;
            if (!_euv1)
            {
                EUV1Expand.IsExpanded = false;
                EUV1Expand.IsEnabled = false;
                EUV2Expand.IsEnabled = true;
                return true;
            }
            EUV1Expand.IsEnabled = true;
            EUV1Expand.IsExpanded = true;
            EUV2Expand.IsEnabled = false;
            return false;
        }

        private void CheckEUVExpand(object sender, RoutedEventArgs e)
        {
            EUV2Expand.IsExpanded = IsEUVExpanded();
        }
    }
}
