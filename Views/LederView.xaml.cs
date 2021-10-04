using GFElevInterview.Data;
using GFElevInterview.Models;
using GFElevInterview.Tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for LederView.xaml
    /// </summary>
    public partial class LederView : UserControl
    {
        private ElevModel elev;
        private static Grid lederOverlayLoading;

        public LederView() {
            InitializeComponent();
            InitialiserView();

            SetEventHandlers();

            InitialiserComboBoxes();
        }

        #region Initialisering af view

        //On Constructor call
        private void InitialiserView() {
            InitialiserDataGrid();

            lederOverlayLoading = overlayLoading;
        }

        /// <summary>
        /// Sætter Eventhandlers på <see cref="Control"/> ved opstart.
        /// </summary>
        private void SetEventHandlers() {
            /* Reset */
            btnReset.Click += ResetButton_Click;
            /* Excel */
            btnExcel.Click += Excel_Click;
            /* Eksport */
            btnExportMerit.Click += Eksporter_Click;
            btnExportRKV.Click += Eksporter_Click;
            /* Database Gets */
            btnSPS.Click += HentElever_Click;
            btnEUD.Click += HentElever_Click;
            btnMerit.Click += HentElever_Click;
            btnRKV.Click += HentElever_Click;
            btnVisAlle.Click += HentElever_Click;
            /* Blanket Gets */
            btnOpen_Merit.Click += HentBlanket_Click;
            btnOpen_RKV.Click += HentBlanket_Click;
            /* TextBox */
            txtSearch.TextChanged += ElevQuery_TextChanged;
            /* ComboBox */
            cmbKategori.SelectionChanged += QueryKategori_SelectionChanged;
            cmbSubkategori.SelectionChanged += QuerySubkategori_SelectionChanged;
            /* Password */
            btnSkiftPassword.Click += SkiftPassword_btnClick;
            btnValiderKodeord.Click += ValiderOpdaterPassword_Click;
            btnTilbage.Click += Button_Click;
            /* DataGrid */
            gridElevTabel.SelectionChanged += ElevTabel_SelectionChanged;
            /* Åben Mappe */
            btnOutputDir.Click += OpenOutputDirectory_Click;
        }

        private void OpenOutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            FilHandler.VisFilIExplorer(false);
        }

        /// <summary>
        /// Initialiserer <see cref="cmbKategori"/> med værdier, og disabler <see cref="cmbSubkategori"/>
        /// </summary>
        private void InitialiserComboBoxes() {
            cmbKategori.ItemsSource = new string[] {
                "Uddannelser",
                "Skole Adresser",
                "Dansk Fagniveau",
                "Engelsk Fagniveau",
                "Matematik Fagniveau"
            };

            cmbSubkategori.IsEnabled = false;
        }

        private void InitialiserDataGrid() {
            OpdaterDataGrid(DbTools.Instance.VisAlle());
        }

        #endregion Initialisering af view

        #region Funktioner

        /// <summary>
        /// Sætter visibility for lederOverlayLaoding, ud fra om den får en true(Usynlig) eller false(Synlig).
        /// </summary>
        /// <param name="harBrugerInput"></param>
        public static void SetBrugerInput(bool harBrugerInput) {
            lederOverlayLoading.Visibility = harBrugerInput ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Opdaterer indholdet i <see cref="gridElevTabel"/>.
        /// </summary>
        /// <param name="elevData">Ny elev data.</param>
        public void OpdaterDataGrid(List<ElevModel> elevData) {
            gridElevTabel.ItemsSource = elevData;
        }

        /// <summary>
        /// Nulstiller viewet og viser alle elever i databasen.
        /// </summary>
        private void VisAlleDataGrid() {
            cmbSubkategori.SelectedIndex = -1;
            OpdaterDataGrid(DbTools.Instance.VisAlle());
        }

        #endregion Funktioner

        #region Datagrid EventHandler

        /// <summary>
        /// Aktiver / Deaktiver funktionalitet baseret på den valgte elev.
        /// </summary>
        private void ElevTabel_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            elev = (sender as DataGrid).SelectedItem as ElevModel;

            if (elev == null) {
                btnOpen_Merit.IsEnabled = false;
                btnOpen_RKV.IsEnabled = false;
                return;
            }

            //NOTE understående burde være det samme som overstående.
            btnOpen_Merit.IsEnabled = elev.DanNiveau > FagNiveau.Null;
            btnOpen_RKV.IsEnabled = elev.ElevType > EUVType.Null;
        }

        #endregion Datagrid EventHandler

        #region Button EventHandler

        /// <summary>
        /// henter elever fra databasen.
        /// </summary>
        private void HentElever_Click(object sender, RoutedEventArgs e) {
            Button btn = sender as Button;

            if (btn == btnSPS) {
                OpdaterDataGrid(DbTools.Instance.VisSPS());
            }
            else if (btn == btnEUD) {
                OpdaterDataGrid(DbTools.Instance.VisEUD());
            }
            else if (btn == btnRKV) {
                OpdaterDataGrid(DbTools.Instance.VisRKV());
            }
            else if (btn == btnMerit) {
                OpdaterDataGrid(DbTools.Instance.VisMerit());
            }
            else {
                cmbSubkategori.SelectedIndex = -1;
                OpdaterDataGrid(DbTools.Instance.VisAlle());
            }
        }

        /// <summary>
        /// Åbner en ny stifinder og viser den valgte fil.
        /// </summary>
        private void HentBlanket_Click(object sender, RoutedEventArgs e) {
            FilHandler.VisFilIExplorer(false, (sender as Button) == btnOpen_Merit ? elev.FilnavnMerit : elev.FilnavnRKV);
        }

        /// <summary>
        /// Eksporter blanketter.
        /// </summary>
        private void Eksporter_Click(object sender, RoutedEventArgs e) {
            switch ((sender as Button) == btnExportMerit) {
                case true:
                    if (AlertBoxes.OnExportMerit()) {
                        if (FilHandler.KombinerMeritFiler()) {
                            FilHandler.VisFilIExplorer(false, StandardVaerdier.SamletMeritFilnavn);
                        }
                        else {
                            AlertBoxes.OnNoDocumentsForExport();
                        }
                    }
                    break;

                case false:
                    if (AlertBoxes.OnExportRKV()) {
                        if (FilHandler.ZipRKVFiler()) {
                            FilHandler.VisFilIExplorer(false, StandardVaerdier.SamletRKVFilNavn);
                        }
                        else {
                            AlertBoxes.OnNoDocumentsForExport();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// EventHandler for tilføjelse af Excel-ark til databasen.
        /// </summary>
        private void Excel_Click(object sender, RoutedEventArgs e) {
            FilHandler.OpenFileDialog();
            VisAlleDataGrid();
        }

        /// <summary>
        /// Tjekker om de indtastede passwords passer over ens med hinanden.
        /// Hvis de passer over ens bliver passwordet opdateret og gemt i databasen,
        /// <br/>CurrentUser bliver NulStillet, En pop op besked bliver vist og brugeren bliver sendt til bage til login siden.
        /// </summary>
        private void ValiderOpdaterPassword_Click(object sender, RoutedEventArgs e) {
            if (txtKodeord.Password == txtValiderKodeord.Password) {
                if (DbTools.Instance.OpdaterPassword(txtKodeord.Password)) {
                    AlertBoxes.OnSuccessfulPasswordChange();
                    CurrentUser.NulstilCurrentUser();
                    MainWindow.Instance.CheckLederEllerLogin();
                }
                else {
                    AlertBoxes.OnUnlikelyError();
                }
            }
            else {
                AlertBoxes.OnFailedMatchingPasswords();
                txtKodeord.Clear();
                txtValiderKodeord.Clear();
            }
        }

        /// <summary>
        /// Sætter værdien for <see cref="SetBrugerInput(bool)"/> til false(Synlig)
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e) {
            SetBrugerInput(true);
        }

        /// <summary>
        /// Sætter værdien for <see cref="SetBrugerInput(bool)"/> til false(Synlig)
        /// </summary>
        private void SkiftPassword_btnClick(object sender, RoutedEventArgs e) {
            SetBrugerInput(false);
        }

        /// <summary>
        /// Nulstiller Elev tabellen i Databasen, og sletter alle merit og RKV blanketter.
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            if (AlertBoxes.OnResettingDatabase()) {
                if (DbTools.Instance.NulstilEleverAlt()) {
                    VisAlleDataGrid();
                    AlertBoxes.OnPostResetDatabase();
                }
            }
        }

        #endregion Button EventHandler

        #region Combobox Eventhandler

        /// <summary>
        /// Håndterer behavior for <see cref="cmbKategori"/>, som indsætter items ind i <see cref="cmbSubkategori"/>.
        /// </summary>
        private void QueryKategori_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int index = (sender as ComboBox).SelectedIndex;
            cmbSubkategori.ItemsSource = null;

            switch (index) {
                case -1:
                    cmbSubkategori.IsEnabled = false;
                    return;

                case 0:
                    cmbSubkategori.ItemsSource = StandardVaerdier.HentUddannelser(false);
                    break;

                case 1:
                    cmbSubkategori.ItemsSource = StandardVaerdier.HentAlleSkoler();
                    break;

                default:
                    cmbSubkategori.ItemsSource = StandardVaerdier.HentFagNiveau();
                    break;
            }
            cmbSubkategori.IsEnabled = true;
        }

        /// <summary>
        /// Håndterer hentning af data fra databasen. Er kun aktiv hvis <see cref="cmbKategori"/> har en værdi.
        /// </summary>
        private void QuerySubkategori_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int index = cmbKategori.SelectedIndex;

            if (index < 0 || cmbSubkategori.SelectedIndex < 0) {
                VisAlleDataGrid();
                return;
            }

            List<ElevModel> elever;
            string text = cmbSubkategori.SelectedItem.ToString();

            switch (index) {
                case 0:
                    elever = DbTools.Instance.VisUddannelse(text);
                    break;

                case 1:
                    elever = DbTools.Instance.VisSkole(text);
                    break;

                default:
                    elever = DbTools.Instance.VisFagNiveau(cmbKategori.SelectedItem.ToString(), (FagNiveau)cmbSubkategori.SelectedIndex + 1);
                    break;
            }

            OpdaterDataGrid(elever);
        }

        #endregion Combobox Eventhandler

        #region TextBox EventHandler

        /// <summary>
        /// Opdaterer DataGrid når indholdet af TextBox bliver ændret.
        /// </summary>
        private void ElevQuery_TextChanged(object sender, TextChangedEventArgs e) {
            string query = (sender as TextBox).Text;
            //Nulstil datagrid

            if (string.IsNullOrEmpty(query)) {
                VisAlleDataGrid();
                return;
            }

            OpdaterDataGrid(DbTools.Instance.VisQueryElever(query));
        }

        #endregion TextBox EventHandler
    }
}