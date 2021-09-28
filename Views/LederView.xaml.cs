using GFElevInterview.Data;
using GFElevInterview.Models;
using GFElevInterview.Tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BC = BCrypt.Net.BCrypt;

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
            InitialiserSkoleComboBox();
            InitialiserUddannelsesComboBox();

            btnReset.Click += ResetButton_Click;

            /* ny select function */
            btnSPS.Click += Select_btnClick;
            btnEUD.Click += Select_btnClick;
            btnMerit.Click += Select_btnClick;
            btnRKV.Click += Select_btnClick;
            btnVisAlle.Click += Select_btnClick;

            Sogefelt.TextChanged += Query_TekstInput;
        }

        //On Constructor call
        private void InitialiserView() {
            InitialiserDataGrid();

            lederOverlayLoading = overlayLoading;
        }

        private void InitialiserDataGrid() {
            OpdaterDataGrid(DbTools.Instance.VisAlle());
        }

        private void InitialiserSkoleComboBox() {
            cmbSchool.ItemsSource = StandardVaerdier.HentAlleSkoler();
            cmbUddanelse.ItemsSource = StandardVaerdier.HentUddannelser(false);
        }

        private void InitialiserUddannelsesComboBox() {
            cmbUddanelse.ItemsSource = StandardVaerdier.HentUddannelserCmb();
        }

        public void OpdaterDataGrid(List<ElevModel> elevData) {
            gridElevTabel.ItemsSource = elevData;
        }

        /// <summary>
        /// Åbner en ny stifinder og viser den valgte fil.
        /// </summary>
        /// <param name="blanketNavn">filnavnet på blanketten.</param>
        private void ÅbenFilPlacering(string blanketNavn) {
            FilHandler.VisBlanketIExplorer(blanketNavn);
        }

        //TODO ryk ind i Tools/Filhandler hvis muligt.
        private void ÅbenFil() {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            openFile.Filter = "Excel File |*.xls;*.xlsx;*.xlsm";

            Nullable<bool> result = openFile.ShowDialog();
            if ((bool)result) {
                //ProcessstartInfo bruges til at køre python scriptet.
                ProcessStartInfo info = new ProcessStartInfo();
                List<ElevModel> elever = new List<ElevModel>();

                //Python filen bliver hentet ned til filename fil lokationen.
                //info.FileName = ".venv\\Scripts\\python.exe";
                info.FileName = RessourceFil.pythonScript;
                //Python scripted bliver hentet og kørt ved hjælp af fileName.
                info.Arguments = string.Format("\"{0}\"", openFile.FileName);

                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.CreateNoWindow = true;
                info.UseShellExecute = false;

                //processeren bliver kørt, med informationerne fra ProcessStartInfo.
                using (Process process = Process.Start(info)) {
                    using (StreamReader reader = process.StandardOutput) {
                        //data´en fra reader(python) bliver overført til linje(string) en linje adgangen så længe den ikke finde et null.
                        string linje;
                        while ((linje = reader.ReadLine()) != null) {
                            //Data´en fra linje, bliver splittet op i et string array.
                            string[] elev = linje.Split(';');  //Note: hardCoded seperator
                            if (elev.Length < 2) {
                                AlertBoxes.OnExcelReadingError(linje);
                                return;
                            }
                            //Data´en fra String Array´et bliver tilføjet til elev listen.
                            elever.Add(new ElevModel(elev[0], elev[1], elev[2]));
                        }
                    }
                }
                //DbTools TilføjElever bliver kaldt, hvorefter at eleverne bliver tilføjet til databasen.
                DbTools.Instance.TilføjElever(elever);
                btnVisAlle.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        #region Events

        #region Knap metoder

        private void Select_btnClick(object sender, RoutedEventArgs e) {
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
                cmbSchool.SelectedIndex = -1;
                OpdaterDataGrid(DbTools.Instance.VisAlle());
            }
        }

        private void VisAlleDataGrid() {
            cmbSchool.SelectedIndex = -1;
            OpdaterDataGrid(DbTools.Instance.VisAlle());
        }

        #endregion Knap metoder

        #region TODO Implementer HentBlanket_btnClick

        //TODO implementer i stedet for at have 2 andre
        private void HentBlanket_btnClick(object sender, RoutedEventArgs e) {
            ÅbenFilPlacering((sender as Button) == btnOpen_Merit ? elev.FilnavnMerit : elev.FilnavnRKV);
        }

        private void Open_Merit_Click(object sender, RoutedEventArgs e) {
            ÅbenFilPlacering(elev.FilnavnMerit);
        }

        private void Open_RKV_Click(object sender, RoutedEventArgs e) {
            //TODO Reduce redundancy
            //if (elev == null)
            //{
            //    return;
            //}
            ÅbenFilPlacering(elev.FilnavnRKV);
        }

        #endregion TODO Implementer HentBlanket_btnClick

        #region TODO implementer Eksporter_btnClick

        private void Eksporter_btnClick(object sender, RoutedEventArgs e) {
            switch ((sender as Button) == btnOpen_Merit) {
                case true:
                    if (AlertBoxes.OnExportMerit()) {
                        FilHandler.KombinerMeritFiler();
                    }
                    break;

                case false:
                    if (AlertBoxes.OnExportRKV()) {
                        FilHandler.ZipRKVFiler();
                    }
                    break;
            }
        }

        private void ExportMerit_Click(object sender, RoutedEventArgs e) {
            if (AlertBoxes.OnExportMerit()) {
                FilHandler.KombinerMeritFiler();
            }
        }

        private void ExportRKV_Click(object sender, RoutedEventArgs e) {
            if (AlertBoxes.OnExportRKV()) {
                FilHandler.ZipRKVFiler();
            }
        }

        #endregion TODO implementer Eksporter_btnClick

        private void SkoleDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if ((sender as ComboBox).SelectedIndex == -1) {
                return;
            }
            string skole = (sender as ComboBox).SelectedItem.ToString();

            if (skole.Contains(' ')) {
                string ændretSkole = skole.Substring(0, skole.IndexOf(' '));
                Console.WriteLine();
                //Merit forløb
                if (skole.Contains('+')) {
                    //Tekst fra skole variablen, fra start til mellemrummet.
                    //Også skal vi sætte det rigtige fagniveau.
                    OpdaterDataGrid(DbTools.Instance.VisSkole(ændretSkole, FagNiveau.F, true));
                }
                //Ingen merit
                else {
                    OpdaterDataGrid(DbTools.Instance.VisSkole(ændretSkole, FagNiveau.E, false));
                }
            }
            else {
                OpdaterDataGrid(DbTools.Instance.VisSkole(skole));
            }
        }

        private void cmbUddanelse_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            OpdaterDataGrid(DbTools.Instance.VisUddannelse((sender as ComboBox).SelectedItem.ToString()));
        }

        //TODO kan sætte elev som tom række
        private void elevTabel_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            elev = (sender as DataGrid).SelectedItem as ElevModel;

            if (elev == null) {
                btnOpen_Merit.IsEnabled = false;
                btnOpen_RKV.IsEnabled = false;
                return;
            }

            if (elev.DanNiveau == FagNiveau.Null)
                btnOpen_Merit.IsEnabled = false;
            else
                btnOpen_Merit.IsEnabled = true;

            if (elev.ElevType == EUVType.Null)
                btnOpen_RKV.IsEnabled = false;
            else
                btnOpen_RKV.IsEnabled = true;

            //NOTE understående burde være det samme som overstående.
            btnOpen_Merit.IsEnabled = elev.DanNiveau > FagNiveau.Null;
            btnOpen_RKV.IsEnabled = elev.ElevType > EUVType.Null;
        }

        private void Query_TekstInput(object sender, TextChangedEventArgs e) {
            string query = (sender as TextBox).Text;
            //Nulstil datagrid

            if (string.IsNullOrEmpty(query)) {
                VisAlleDataGrid();
                return;
            }

            OpdaterDataGrid(DbTools.Instance.VisQueryElever(query));
        }

        #endregion Events

        private void TilføjKnp_Click(object sender, RoutedEventArgs e) {
            ÅbenFil();
        }

        /// <summary>
        /// Tjekker om de indtastede passwords passer over ens med hinanden.
        /// Hvis de passer over ens bliver passwordet opdateret og gemt i databasen,
        /// <br/>CurrentUser bliver NulStillet, En pop op besked bliver vist og brugeren bliver sendt til bage til login siden.
        /// </summary>
        private void ValiderOpdaterPassword_btnClick(object sender, RoutedEventArgs e) {
            if (txtKodeord.Text == txtValiderKodeord.Text) {
                if (DbTools.Instance.OpdaterPassword(txtKodeord.Text)) {
                    AlertBoxes.OnSuccessfulPasswordChange();
                    CurrentUser.NulstilCurrentUser();
                    MainWindow.Instance.CheckLederEllerLogin();
                }
                else {
                    //TODO
                    MessageBox.Show("Nej");
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
        /// Sætter visibility for lederOverlayLaoding, ud fra om den får en true(Usynlig) eller false(Synlig).
        /// </summary>
        /// <param name="harBrugerInput"></param>
        public static void SetBrugerInput(bool harBrugerInput) {
            lederOverlayLoading.Visibility = harBrugerInput ? Visibility.Collapsed : Visibility.Visible;
        }

        //TODO Alertboxes
        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            if (AlertBoxes.OnExportMerit()) {
                if (DbTools.Instance.NulstilEleverAlt()) {
                    VisAlleDataGrid();
                    MessageBox.Show("SUC RESET");
                }
            }
        }
    }
}