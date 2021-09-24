using GFElevInterview.Data;
using GFElevInterview.Models;
using GFElevInterview.Tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public LederView() {
            InitializeComponent();
            InitialiserView();
            InitialiserSkoleComboBox();

            btnReset.Click += ResetButton_Click;
        }

        //On Constructor call
        private void InitialiserView() {
            InitialiserDataGrid();
        }

        private void InitialiserDataGrid() {
            OpdaterDataGrid(DbTools.Instance.VisAlle());
        }

        //Putter info ind fra App.Config i ComboBox
        private void InitialiserSkoleComboBox() {
            cmbSchool.ItemsSource = StandardVaerdier.HentAlleSkoler();
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
            ÅbenFilPlacering(elev.MeritFilNavn);
        }

        private void Open_RKV_Click(object sender, RoutedEventArgs e) {
            //TODO Reduce redundancy
            //if (elev == null)
            //{
            //    return;
            //}
            ÅbenFilPlacering(elev.RKVFilNavn);
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

        //TODO kan sætte elev som tom række
        private void elevTabel_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            elev = (sender as DataGrid).SelectedItem as ElevModel;

            if (elev == null) {
                btnOpen_Merit.IsEnabled = false;
                btnOpen_RKV.IsEnabled = false;
                return;
            }

            if (elev.danskNiveau == FagNiveau.Null)
                btnOpen_Merit.IsEnabled = false;
            else
                btnOpen_Merit.IsEnabled = true;

            if (elev.elevType == ElevType.Null)
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