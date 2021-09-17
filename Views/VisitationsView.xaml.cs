﻿using GFElevInterview.Data;
using GFElevInterview.Interfaces;
using GFElevInterview.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for WordView.xaml
    /// </summary>
    public partial class VisitationsView : UserControl, IBlanket
    {
        /* Fields */
        BlanketView parent;

        /* Constructor */
        public VisitationsView(BlanketView parent) {
            InitializeComponent();
            this.parent = parent;
            InitialiserBlanket();
            cmbEducation.DropDownClosed += Combobox_DropDownClosed;
            cmbAdresse.DropDownClosed += Combobox_DropDownClosed;
        }


        #region Klargøringsmetoder
        private void InitialiserBlanket() {
            SætKnapper();
            SætKomboBokse();

            UdfyldBlanketHvisAlleredeEksisterende();
        }

        //TODO Ændre måden vi henter lister
        private void SætKomboBokse() {
            cmbAdresse.ItemsSource = CurrentElev.elev.ValgAfSkoler();
            cmbEducation.ItemsSource = CurrentElev.elev.ValgAfUddannelser();
        }

        private void SætKnapper() {
            parent.btnFrem.Content = "Gem";
            parent.btnTilbage.IsEnabled = true;
        }

        /// <summary>
        /// udfyldning af visitationsView
        /// </summary>
        private void UdfyldBlanketHvisAlleredeEksisterende() {

            if (!String.IsNullOrEmpty(CurrentElev.elev.uddannelse))
            {
                UdfyldBlanket.UdfyldRadioButton(rbSpsJa, rbSpsNej, CurrentElev.elev.sps);
                UdfyldBlanket.UdfyldRadioButton(rbEudJa, rbEudNej, CurrentElev.elev.eud);

                UdfyldBlanket.UdfyldComboBox(cmbEducation, CurrentElev.elev.uddannelse);
                UdfyldBlanket.UdfyldComboBox(cmbAdresse, CurrentElev.elev.uddannelseAdresse);
            }

                return;
            if (!String.IsNullOrEmpty(CurrentElev.elev.uddannelse)) {
                cmbEducation.SelectedItem = CurrentElev.elev.uddannelse;
            }

            if (!String.IsNullOrEmpty(CurrentElev.elev.uddannelseAdresse)) {
                //Hvis der er ændret i Dansk karakter, så vil hverken Lyngby eller Frederiksberg blive vist.
                if (cmbEducation.Items.Contains(CurrentElev.elev.uddannelseAdresse)) {
                    CurrentElev.elev.uddannelseAdresse = null;
                    cmbAdresse.SelectedItem = CurrentElev.elev.uddannelseAdresse;
                }
            }


            switch (CurrentElev.elev.sps) {
                case true:
                    rbSpsJa.IsChecked = true;
                    break;
                case false:
                    rbSpsNej.IsChecked = true;
                    break;
                default:
                    break;
            }

            switch (CurrentElev.elev.eud) {
                case true:
                    rbEudJa.IsChecked = true;
                    break;
                case false:
                    rbEudNej.IsChecked = true;
                    break;
                default:
                    break;
            }
        }
        #endregion

        public void Frem() {
            if (ErValideret()) {
                SætCurrentElevVærdier();
                parent.FærdiggørInterview();
            }
        }
        /// <summary>Ændr <see cref="BlanketView"/>s <see cref="ContentControl"/> til <see cref="MeritBlanketView"/></summary>
        public void Tilbage()
        {
            parent.SkiftBlanket(new MeritBlanketView(parent));
        }

        //TODO
        private void SætCurrentElevVærdier() {
            //NOTE: Bliver sat før vi overhovedet kommer hertil

            CurrentElev.elev.uddannelseAdresse = cmbAdresse.Text;
            CurrentElev.elev.uddannelse = cmbEducation.Text;
            CurrentElev.elev.sps = (bool)rbSpsJa.IsChecked;
            CurrentElev.elev.eud = (bool)rbEudJa.IsChecked;
        }


        /// <summary>
        /// Bestemmer om siden er valideret og klar til afslutning, og highlighter felter der mangler.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is validated; otherwise, <c>false</c>.
        /// </returns>
        private bool ErValideret() {
            SolidColorBrush gray = Brushes.Gray;
            SolidColorBrush red = Brushes.Red;

            bool _educationArea = cmbEducation.SelectedIndex >= 0;
            bool _educationAdresse = cmbAdresse.SelectedIndex >= 0;
            bool _spsSupport = (bool)rbSpsJa.IsChecked || (bool)rbSpsNej.IsChecked;
            bool _eudSupport = (bool)rbEudJa.IsChecked || (bool)rbEudNej.IsChecked;

            //Farv Boxen Grå hvis den er udfyldt eller rød hvis ikke.
            bdrEducation.BorderBrush = _educationArea ? gray : red;
            bdrAdresse.BorderBrush = _educationAdresse ? gray : red;
            bdrSps.BorderBrush = _spsSupport ? gray : red;
            bdrEud.BorderBrush = _eudSupport ? gray : red;

            if (_educationArea && _educationAdresse && _spsSupport && _eudSupport) {

                return true;
            }
            return false;
        }

        //Events
        #region Combobox/Radiobutton Eventhandlers
        #region Combobox
        /// <summary>Fjerner fokus fra combobox når den folder sammen.</summary>
        private void Combobox_DropDownClosed(object sender, EventArgs e) {
            parent.scroll.Focus();
        }

        /// <summary>Sætter <see cref="CurrentElev.elev"/> værdi på valg fra <see cref="ComboBox"/></summary>
        private void educationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //CurrentElev.elev.uddannelse = (sender as ComboBox).SelectedItem.ToString();
        }

        /// <summary>Sætter <see cref="CurrentElev.elev"/> værdi på valg fra <see cref="ComboBox"/></summary>
        private void educationAdresseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //CurrentElev.elev.uddannelseAdresse = (sender as ComboBox).SelectedItem.ToString();
        }
        #endregion Combobox

        #region RadioButton setters
        private void SPSSupport_Checked(object sender, RoutedEventArgs e) {

           // CurrentElev.elev.sps = (sender as RadioButton) == rbSpsJa ? true : false;
        }

        private void EUDSupport_Checked(object sender, RoutedEventArgs e) {
            //CurrentElev.elev.eud = (sender as RadioButton) == rbEudJa ? true : false;
        }
        #endregion Radiobutton setters
        #endregion
    }
}
