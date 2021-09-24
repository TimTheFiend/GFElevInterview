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

        private void InitialiserUddannelsesComboBox()
        {
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
            string currentDir = Directory.GetCurrentDirectory();
            int index = currentDir.LastIndexOf('\\');  //finder positionen på sidste "\" i current dir
            //kombinerer strings til at give os filstien på den valgte pdf
            string filSti = Path.Combine(currentDir.Substring(0, index), RessourceFil.outputMappeNavn, blanketNavn);

            Process.Start("explorer.exe", $"/select,\"{filSti}");  //"/select," highlighter den valgte fil.
        }

        ////TODO @Joakim Doku
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

        private void SPS_Click(object sender, RoutedEventArgs e) {
            OpdaterDataGrid(DbTools.Instance.VisSPS());
        }

        private void EUD_Click(object sender, RoutedEventArgs e) {
            OpdaterDataGrid(DbTools.Instance.VisEUD());
        }

        private void RKV_Click(object sender, RoutedEventArgs e) {
            OpdaterDataGrid(DbTools.Instance.VisRKV());
        }

        private void Merit_Click(object sender, RoutedEventArgs e) {
            OpdaterDataGrid(DbTools.Instance.VisMerit());
        }

        private void visAlle_Click(object sender, RoutedEventArgs e) {
            cmbSchool.SelectedIndex = -1;
            OpdaterDataGrid(DbTools.Instance.VisAlle());
        }

        #endregion Knap metoder

        private void Open_Merit_Click(object sender, RoutedEventArgs e) {
            //TODO Reduce redundancy
            //if (elev == null)
            //{
            //    return;
            //}
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

            //Er eleven færdig med interview?
            //Hvis ja, enable knap,
            //Hvis nej, disable knap.
        }

        #endregion Events

        private void TilføjKnp_Click(object sender, RoutedEventArgs e) {
            ÅbenFil();
        }

        //TODO DOKU Victor
        /// <summary>
        /// Tjekker om de indtastede passwords passer over ens med hinanden.
        /// Hvis de passer over ens bliver passwordet opdateret og gemt i databasen,
        /// <br/>CurrentUser bliver NulStillet, En pop op besked bliver vist og brugeren bliver sendt til bage til login siden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        //TODO @Victor Doku + Navneændring
        /// <summary>
        /// Sætter værdien for <see cref="SetBrugerInput(bool)"/> til false(Synlig)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e) {
            SetBrugerInput(true);
        }

        //TODO @Victor Doku
        /// <summary>
        /// Sætter værdien for <see cref="SetBrugerInput(bool)"/> til false(Synlig)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkiftPassword_btnClick(object sender, RoutedEventArgs e) {
            SetBrugerInput(false);
        }

        //TODO @Victor Doku
        //TODO @Joakim overvej at rykke metode
        /// <summary>
        /// Sætter visibility for lederOverlayLaoding, ud fra om den får en true(Usynlig) eller false(Synlig).
        /// </summary>
        /// <param name="harBrugerInput"></param>
        public static void SetBrugerInput(bool harBrugerInput) {
            lederOverlayLoading.Visibility = harBrugerInput ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            if (AlertBoxes.OnExportMerit()) {
                if (DbTools.Instance.NulstilEleverAlt()) {
                    visAlle_Click(btnVisAlle, new RoutedEventArgs());
                    MessageBox.Show("SUC RESET");
                }
            }
        }
    }
}