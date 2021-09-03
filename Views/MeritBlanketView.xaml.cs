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
            InitialiserBlanket();
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
        private void InitialiserBlanket() {
            InitialiserComboBox();
            SætButtons();
            // Udfyld Blanketten hvis den allerede står som udfyldt i `CurrentElev`.
            if (CurrentElev.elev.ErUdfyldt) {
                //Combobox
                ComboboxDansk.SelectedIndex = (int)CurrentElev.elev.danskNiveau - 1;  // -1 pga `Null` ikke er en del af comboboksen
                ComboboxEngelsk.SelectedIndex = (int)CurrentElev.elev.engelskNiveau - 1;
                ComboboxMatematik.SelectedIndex = (int)CurrentElev.elev.matematikNiveau - 1;
                //Checkbox
                DanskEksamenJa.IsChecked = CurrentElev.elev.danskEksammen;
                DanskUndervisJa.IsChecked = CurrentElev.elev.danskUndervisning;
                MatematikEksamenJa.IsChecked = CurrentElev.elev.matematikEksammen;
                MatematikUndervisJa.IsChecked = CurrentElev.elev.matematikUndervisning;
                EngelskEksamenJa.IsChecked = CurrentElev.elev.engelskEksammen;
                EngelskUndervisJa.IsChecked = CurrentElev.elev.engelskUndervisning;

            }
        }

        /// <summary>
        /// Fylder ComboBoxene op med karaktere.
        /// </summary>
        private void InitialiserComboBox()
        {
            var enumKeysArray = Enum.GetNames(typeof(FagNiveau)).Where(x => x != FagNiveau.Null.ToString());
            ComboboxDansk.ItemsSource = enumKeysArray;
            ComboboxEngelsk.ItemsSource = enumKeysArray;
            ComboboxMatematik.ItemsSource = enumKeysArray;

            UdfyldBlanket();
        }

        private void UdfyldBlanket()
        {
            if(ComboboxDansk == null)
            {
                ComboboxDansk.SelectedItem = CurrentElev.elev.danskNiveau;
            }
            if(ComboboxEngelsk == null)
            {
                ComboboxEngelsk.SelectedItem = CurrentElev.elev.engelskNiveau;
            }
            if(ComboboxMatematik == null)
            {
                ComboboxMatematik.SelectedItem = CurrentElev.elev.matematikNiveau;
            }

            //Opdater blanket så den passer med currentelev.
            //vi naviger tilbage fra Vis/RKV blanket.
            if (CurrentElev.elev.danskNiveau != FagNiveau.Null)
            {
                switch (CurrentElev.elev.danskEksammen)
                {
                    case true:
                        DanskEksamenJa.IsChecked = true;
                        break;
                    case false:
                        DanskEksamenNej.IsChecked = true;
                        break;
                }
                switch (CurrentElev.elev.engelskEksammen)
                {
                    case true:
                        EngelskEksamenJa.IsChecked = true;
                        break;
                    case false:
                        EngelskEksamenNej.IsChecked = true;
                        break;
                }
                switch (CurrentElev.elev.matematikEksammen)
                {
                    case true:
                        MatematikEksamenJa.IsChecked = true;
                        break;
                    case false:
                        MatematikEksamenNej.IsChecked = true;
                        break;
                }
                switch (CurrentElev.elev.danskUndervisning)
                {
                    case true:
                        DanskUndervisJa.IsChecked = true;
                        break;
                    case false:
                        DanskUndervisNej.IsChecked = true;
                        break;
                }
                switch (CurrentElev.elev.engelskUndervisning)
                {
                    case true:
                        EngelskUndervisJa.IsChecked = true;
                        break;
                    case false:
                        EngelskUndervisNej.IsChecked = true;
                        break;
                }
                switch (CurrentElev.elev.matematikUndervisning)
                {
                    case true:
                        MatematikUndervisJa.IsChecked = true;
                        break;
                    case false:
                        MatematikUndervisNej.IsChecked = true;
                        break;
                }
            }
        }

        private void SætButtons() {
            // Bliver ændret i VisitationsView
            parent.btnFrem.Content = "Frem";

            parent.btnTilbage.IsEnabled = false;
            parent.btnFrem.IsEnabled = true;
        }


        #endregion

        /// <summary>
        /// Håndterer <see cref="BlanketView"/> knap funktion.
        /// </summary>
        public void Frem() {
            // Tjekker om fag niveau er blevet valgt, da det er det eneste vi med sikkerhed ved at eleven kan have.
            if (ErValideret()) {
                IBlanket newView;
                OpdaterElev();
                if (CurrentElev.elev.erRKV)
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

        private void OpdaterElev()
        {
            //NOTE: Bliver sat før vi overhovedet kommer hertil

            CurrentElev.elev.danskEksammen = (bool)DanskEksamenJa.IsChecked;
            CurrentElev.elev.engelskEksammen = (bool)EngelskEksamenJa.IsChecked;
            CurrentElev.elev.matematikEksammen = (bool)MatematikEksamenJa.IsChecked;
            CurrentElev.elev.danskUndervisning = (bool)DanskUndervisJa.IsChecked;
            CurrentElev.elev.engelskUndervisning = (bool)EngelskUndervisJa.IsChecked;
            CurrentElev.elev.matematikUndervisning = (bool)MatematikUndervisJa.IsChecked;
            CurrentElev.elev.danskNiveau = (FagNiveau)ComboboxDansk.SelectedIndex + 1;
            CurrentElev.elev.engelskNiveau = (FagNiveau)ComboboxEngelsk.SelectedIndex + 1;
            CurrentElev.elev.matematikNiveau = (FagNiveau)ComboboxMatematik.SelectedIndex + 1;

            //parent.CompleteCurrentInterview();
        }

        /// <summary>
        /// check at der skal have værdi 
        /// </summary>
        /// <returns><c>true</c> hvis valideret; ellers <c>false</c></returns>
        private bool ErValideret() {

            bool erValideret = true;
            Control[] danskControls = new Control[]
            {
                ComboboxDansk,
                DanskEksamenJa,
                DanskEksamenNej,
                DanskUndervisJa,
                DanskUndervisNej
            };
            Control[] engelskControls = new Control[]
            {
                ComboboxEngelsk,
                EngelskEksamenJa,
                EngelskEksamenNej,
                EngelskUndervisJa,
                EngelskUndervisNej
            };
            Control[] matematikControls = new Control[]
            {
                ComboboxMatematik,
                MatematikEksamenJa,
                MatematikEksamenNej,
                MatematikUndervisJa,
                MatematikUndervisNej
            };

            #region Dropdown menu
            if (!ValiderFag(DanskValidation, danskControls))
                erValideret = false;
            if (!ValiderFag(EngelskValidation, engelskControls))
                erValideret = false;
            if (!ValiderFag(MatematikValidation, matematikControls))
                erValideret = false;


            #endregion
            return erValideret;
        }

        //[Combobox, eksamenJa, eksamenNej, UndervisJa, undervisNej]
        private bool ValiderFag(Border border, Control[] control)
        {
            System.Windows.Media.SolidColorBrush brushTrue = System.Windows.Media.Brushes.Gray;
            System.Windows.Media.SolidColorBrush brushFalse = System.Windows.Media.Brushes.Red;
            bool erValideret = true;

            ComboBox comboBox = control[0] as ComboBox;
            if (comboBox.SelectedIndex == -1)
            {
                border.BorderBrush = brushFalse;
                erValideret = false;
            }
            else
            {
                border.BorderBrush = brushTrue;
            }

            for (int i = 0; i < 2; i++)
            {
                //første omgang = 0 * 2 = 0 + 1 = 1.
                //anden omgang = 1 * 2 = 2 + 1 = 3;
                RadioButton radioJa = control[i * 2 + 1] as RadioButton;
                RadioButton radioNej = control[i * 2 + 2] as RadioButton;

                if (!(bool)radioJa.IsChecked && !(bool)radioNej.IsChecked)
                {
                    border.BorderBrush = brushFalse;
                    erValideret = false;
                }
            }

            return erValideret;
        }

        //Events
        private void Combobox_DropDownClosed(object sender, EventArgs e)
        {
            //Change the focus to scrollview in BlanketView.
            parent.scrollview.Focus();
        }
        // TODO
        ///Mangel på obejct refernce i CurrentElev.elev.niveau
        ///CurrentElev.elev virker til at have mangel på informationer, hvilket kunne skyldes 
        private void ComboboxFagNiveau_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            int selectedIndex = cb.SelectedIndex + 1;

            if (cb == ComboboxDansk)
            {
                CurrentElev.elev.danskNiveau = (FagNiveau)selectedIndex;
            }
            else if (cb == ComboboxEngelsk)
            {
                CurrentElev.elev.engelskNiveau = (FagNiveau)selectedIndex;
            }
            else if (cb == ComboboxMatematik)
            {
                CurrentElev.elev.matematikNiveau = (FagNiveau)selectedIndex;
            }
        }     
    }
}
