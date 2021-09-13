﻿using GFElevInterview.Data;
using GFElevInterview.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using config = System.Configuration.ConfigurationManager;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for LederView.xaml
    /// </summary>
    public partial class LederView : UserControl
    {
        private ElevModel elev;
        private string blanketMappe;

        public LederView() {
            InitializeComponent();
            InitialiserView();
            InitialiserSkoleComboBox();

            blanketMappe = config.AppSettings.Get("outputMappe");
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
            List<string> uddannelsesAdresser = new List<string>() {
                config.AppSettings.Get("ballerup"),
                config.AppSettings.Get("lyngby"),
                config.AppSettings.Get("frederiksberg"),
                config.AppSettings.Get("ballerupMerit"),
                config.AppSettings.Get("ballerupFuldt")
            };
            cmbSchool.ItemsSource = uddannelsesAdresser;
        }

        public void OpdaterDataGrid(List<ElevModel> elevData) {
            gridElevTabel.ItemsSource = elevData;
        }

        private void ÅbenFilPlacering(string blanketNavn) {
            string filNavn = System.IO.Path.Combine(blanketMappe, blanketNavn);

            if (File.Exists(filNavn)) {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{filNavn}");
            }
            else {
                AlertBoxes.OnOpenFileFailure();
            }
        }

        private void ÅbenFil() {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            openFile.Filter = "Excel File |*.xls;*.xlsx;*.xlsm";

            Nullable<bool> result = openFile.ShowDialog();
            if ((bool)result) {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                List<ElevModel> elever = new List<ElevModel>();  //Bruges til at holde på dataen fra `excelExe`

                processStartInfo.FileName = config.AppSettings.Get("excelExe");  //.exe-filen til læsning af excel bliver hentet.
                processStartInfo.Arguments = string.Format("\"{0}\" \"{0}\"", openFile.FileName);  //Argumenter bliver sat

                #region ProcessStartInfo settings

                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;

                #endregion ProcessStartInfo settings

                //processeren bliver kørt, med informationerne fra ProcessStartInfo.
                using (Process process = Process.Start(processStartInfo)) {
                    using (StreamReader reader = process.StandardOutput) {
                        //data´en fra reader(python) bliver overført til linje(string) en linje adgangen så længe den ikke finde et null.
                        string linje;
                        while ((linje = reader.ReadLine()) != null) {
                            //Data´en fra linje, bliver splittet op i et string array.
                            //FIXME stopper hvis der er en linje som ikke passer med setup
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
                DbTools.Instance.TilføjElever(elever);
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
            ÅbenFilPlacering(elev.MeritFilNavn);
        }

        private void Open_RKV_Click(object sender, RoutedEventArgs e) {
            ÅbenFilPlacering(elev.RKVFilNavn);
        }

        private void ExportMerit_Click(object sender, RoutedEventArgs e) {
            if (AlertBoxes.OnExport()) {
                AdminTools.KombinerMeritFiler();
            }
        }

        private void ExportRKV_Click(object sender, RoutedEventArgs e) {
            if (AlertBoxes.OnExport()) {
                AdminTools.ZipRKVFiler();
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
    }
}