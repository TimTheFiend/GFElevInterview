using GFElevInterview.Data;
using GFElevInterview.Interfaces;
using GFElevInterview.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for WordView.xaml
    /// </summary>
    public partial class VisitationsView : UserControl, IBlanket
    {
        private BlanketView parent;

        public VisitationsView(BlanketView parent) {
            InitializeComponent();
            this.parent = parent;
            InitialiserBlanket();
        }

        #region Klargøringsmetoder

        /// <summary>
        /// Klargører view til brug.
        /// </summary>
        private void InitialiserBlanket() {
            SætKnapper();
            SætKomboBokse();

            //Sæt EventHandlers
            cmbUddannelse.DropDownClosed += Combobox_DropDownClosed;
            cmbAdresse.DropDownClosed += Combobox_DropDownClosed;

            UdfyldBlanketHvisAlleredeEksisterende();
        }

        /// <summary>
        /// Fylder <see cref="ComboBox"/> op med indhold.
        /// </summary>
        private void SætKomboBokse() {

            cmbAdresse.ItemsSource = StandardVaerdier.HentSkoler(CurrentElev.elev.DanNiveau > Models.FagNiveau.F);

            if (cmbAdresse.Items.Count == 1)
            {
                cmbAdresse.SelectedIndex = 0;
            }
            cmbUddannelse.ItemsSource = StandardVaerdier.HentUddannelser(false);

            //cmbUddannelse.ItemsSource = new List<string>() {
            //    RessourceFil.infrastruktur,
            //    RessourceFil.itsupporter,
            //    RessourceFil.programmering,
            //    RessourceFil.vedIkke
            //};
        }

        /// <summary>
        /// Ændrer <see cref="BlanketView"/>s knapper.
        /// </summary>
        private void SætKnapper() {
            parent.btnFrem.Content = "Gem";
            parent.btnTilbage.IsEnabled = true;
        }

        /// <summary>
        /// Udfylder <see cref="VisitationsView"/> hvis <see cref="CurrentElev"/> allerede eksisterer i databasen.
        /// </summary>
        private void UdfyldBlanketHvisAlleredeEksisterende() {
            if (!String.IsNullOrEmpty(CurrentElev.elev.UddLinje)) {
                UdfyldBlanket.UdfyldRadioButton(rbSpsJa, rbSpsNej, CurrentElev.elev.SPS);
                UdfyldBlanket.UdfyldRadioButton(rbEudJa, rbEudNej, CurrentElev.elev.EUD);

                UdfyldBlanket.UdfyldComboBox(cmbUddannelse, CurrentElev.elev.UddLinje);
                UdfyldBlanket.UdfyldComboBox(cmbAdresse, CurrentElev.elev.UddAdr);
            }
        }

        #endregion Klargøringsmetoder

        public void Frem() {
            if (ErValideret()) {
                SætCurrentElevVærdier();
                parent.FærdiggørInterview();
            }
        }

        public void Tilbage() {
            parent.SkiftBlanket(new MeritBlanketView(parent));
        }

        /// <summary>
        /// Sætter de passende værdier ind i <see cref="CurrentElev"/>.
        /// </summary>
        private void SætCurrentElevVærdier() {
            CurrentElev.elev.UddAdr = cmbAdresse.Text;
            CurrentElev.elev.UddLinje = cmbUddannelse.Text;
            CurrentElev.elev.SPS = (bool)rbSpsJa.IsChecked;
            CurrentElev.elev.EUD = (bool)rbEudJa.IsChecked;
        }

        /// <summary>
        /// Bestemmer om siden er valideret og klar til afslutning, og highlighter felter der mangler.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is validated; otherwise, <c>false</c>.
        /// </returns>
        private bool ErValideret() {
            return InputValidering.ValiderToRadioButtons(rbSpsJa, rbSpsNej, bdrSps)
                && InputValidering.ValiderToRadioButtons(rbEudJa, rbEudNej, bdrEud)
                && InputValidering.ValiderComboBox(cmbUddannelse, bdrEducation)
                && InputValidering.ValiderComboBox(cmbAdresse, bdrAdresse);
        }

        /// <summary>
        /// Fjerner fokus fra combobox når den folder sammen.
        /// </summary>
        private void Combobox_DropDownClosed(object sender, EventArgs e) {
            parent.scroll.Focus();
        }
    }
}