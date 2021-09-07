﻿using GFElevInterview.Data;
using GFElevInterview.Models;
using GFElevInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            InitialiserBlanket();         
            //TODO
            euv1Ja.Click += CheckEUVUdvidet;
            euv1Nej.Click += CheckEUVUdvidet;
            euv1SporgsmalJa.Click += CheckEUVUdvidet;
            euv1SporgsmalNej.Click += CheckEUVUdvidet;
            educationComboBox.DropDownClosed += Combobox_DropDownClosed;
            uddannelsesBox.DropDownClosed += Combobox_DropDownClosed;
        }

        private void InitialiserBlanket()
        {
            SætButtons();

            uddannelsesBox.ItemsSource = CurrentElev.elev.ValgAfSkoler();
            if(uddannelsesBox.Items.Count == 1)
            {
                uddannelsesBox.SelectedIndex = 0;
            }
            euv1Expand.IsExpanded = false;
            euv1Expand.IsEnabled = false;
            euv2Expand.IsExpanded = false;
            euv2Expand.IsEnabled = false;
            educationComboBox.ItemsSource = CurrentElev.elev.ValgAfUddannelser();
        }

        private void SætButtons()
        {
            parent.btnFrem.Content = "Gem";
            parent.btnTilbage.IsEnabled = true;
        }

        public void Frem()
        {
           if(ErValideret())
           {
                SætElevType();
                CurrentElev.elev.uddannelse = educationComboBox.Text.ToString();
                CurrentElev.elev.uddannelseAdresse = uddannelsesBox.Text.ToString();
                CurrentElev.elev.sps = spsSupportJa.IsChecked;
                CurrentElev.elev.eud = eudSupportJa.IsChecked;
                parent.FærdiggørInterview();
           }
        }

        private void SætElevType() {
            ElevType elevType;
            
            if ((bool)euv1Ja.IsChecked) {
                elevType = ElevType.EUV1;
            }
            else {
                if ((bool)euv2Ja.IsChecked) {
                    elevType = ElevType.EUV2;
                }
                else {
                    elevType = ElevType.EUV3;
                }
            }

            CurrentElev.elev.elevType = elevType;
        }

        public void Tilbage()
        {
            parent.ChangeView(new MeritBlanketView(parent));
        }


        private bool ErValideret()
        {
            SolidColorBrush gray = Brushes.Gray;
            SolidColorBrush red = Brushes.Red;

            IEnumerable<RadioButton> spsRadioButton = spsSupport.Children.OfType<RadioButton>();
            IEnumerable<RadioButton> eudRadioButton = eudSupport.Children.OfType<RadioButton>();
            //Validation
            bool overAllValidated = true;
            //EUV 1
            bool _euv1 = (bool)euv1Ja.IsChecked || (bool)euv1Nej.IsChecked;
            bool _euv1Spg = (bool)euv1SporgsmalJa.IsChecked || (bool)euv1SporgsmalNej.IsChecked;
            //EUV 2
            bool _euv2 = (bool)euv2Ja.IsChecked || (bool)euv2Nej.IsChecked;
            //Education
            bool _educationArea = educationComboBox.SelectedIndex >= 0;
            bool _educationAdresse = uddannelsesBox.SelectedIndex >= 0;      
            //Support
            bool _spsSupport = (bool)spsSupportJa.IsChecked || (bool)spsSupportNej.IsChecked;
            bool _eudSupport = (bool)eudSupportJa.IsChecked || (bool)eudSupportNej.IsChecked;

            //Farv Boxen Grå hvis den er udfyldt eller rød hvis ikke.
            euv1Area.BorderBrush = _euv1 ? gray: red;  

            educationArea.BorderBrush = _educationArea ? gray : red;
            adresse.BorderBrush = _educationAdresse ? gray : red;

            sps.BorderBrush = _spsSupport ? gray : red;
            eud.BorderBrush = _eudSupport ? gray : red;

            if(!_educationArea || !_educationAdresse || !_euv1 || !_spsSupport || !_eudSupport)
            {               
                overAllValidated = false;
            }
            if (_euv1 && (bool)euv1Ja.IsChecked)
            {
                if (!_euv1Spg)
                {
                    euv1sporgsmalBorder.BorderBrush = _euv1Spg ? gray : red;
                    overAllValidated = false;
                }          
            }
            else if (_euv1 )
            {
                if (!_euv2)
                {
                    euv2Area.BorderBrush = _euv2 ? gray : red;
                    overAllValidated = false;
                }
                //overAllValidated = true;
            }

            //if (_euv1Spg1 && _euv1Spg2 && _euv1Spg3 && _euv1Spg4 && _euv2 && _educationArea && _educationAdresse && _spsSupport && _eudSupport)
            //{
            //    MessageBox.Show("Check");
            //    return true;
            //}
            return overAllValidated;
        }

        private bool ErEUVUdvidet()
        {
            bool _euv1 = (bool)euv1Ja.IsChecked || !(bool)euv1Nej.IsChecked;
            bool _euv1Spg = (bool)euv1SporgsmalJa.IsChecked || !(bool)euv1SporgsmalNej.IsChecked;
            if (!_euv1)
            {
                euv1Expand.IsExpanded = false;
                euv1Expand.IsEnabled = false;
                euv2Expand.IsEnabled = true;
                return true;
            }
            if (!_euv1Spg)
            {
                euv1Expand.IsExpanded = false;
                euv1Expand.IsEnabled = false;
                euv2Expand.IsEnabled = true;
                euv1SporgsmalNej.IsChecked = false;
                euv1Ja.IsChecked = false;
                euv1Nej.IsChecked = true;
                return true;
            }
            euv1Expand.IsEnabled = true;
            euv1Expand.IsExpanded = true;
            euv2Expand.IsEnabled = false;
            return false;
        }
        //Events
        private void Combobox_DropDownClosed(object sender, EventArgs e)
        {
            parent.scrollview.Focus();
        }

        private void CheckEUVUdvidet(object sender, RoutedEventArgs e)
        {
            euv2Expand.IsExpanded = ErEUVUdvidet();
        }
    }
}
