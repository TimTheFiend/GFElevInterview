using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GFElevInterview.Models;
using GFElevInterview.Data;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.IO;
using config = System.Configuration.ConfigurationManager;
using Microsoft.Win32;
using System.Diagnostics;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for LederView.xaml
    /// </summary>
    public partial class LederView : UserControl
    {
        private DbTools db;
        private ElevModel elev;
        private string blanketMappe;

        public LederView()
        {
            InitializeComponent();
            InitialiserView();
            InitialiserSkoleComboBox();

            blanketMappe = config.AppSettings.Get("outputMappe");
        }

        //On Constructor call
        private void InitialiserView()
        {
            db = new DbTools();
            InitialiserDataGrid();
        }

        private void InitialiserDataGrid()
        {
            OpdaterDataGrid(new DbTools().VisAlle());
        }

        //Putter info ind fra App.Config i ComboBox
        private void InitialiserSkoleComboBox()
        {
            List<string> uddannelsesAdresser = new List<string>() {
                config.AppSettings.Get("ballerup"),
                config.AppSettings.Get("lyngby"),
                config.AppSettings.Get("frederiksberg"),
                config.AppSettings.Get("ballerupMerit"),
                config.AppSettings.Get("ballerupFuldt")
            };
            SkoleDropDown.ItemsSource = uddannelsesAdresser;
        }

        public void OpdaterDataGrid(List<ElevModel> elevData)
        {
            elevTabel.ItemsSource = elevData;
        }

        private void ÅbenFilPlacering(string blanketNavn)
        {
            string filNavn = System.IO.Path.Combine(blanketMappe, blanketNavn);

            if (File.Exists(filNavn))
            {
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{filNavn}");
            }
            else
            {
                AlertBoxes.OnOpenFileFailure();
            }
        }

        private void ÅbenFil()
        {

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = @"C:\Users\viga\Documents\GitHub\GFElevInterview\bin\Debug\Blanketter";
            //openFile.InitialDirectory = Environment.SpecialFolder.DesktopDirectory;
            
            
            Nullable<bool> result = openFile.ShowDialog();
            if((bool) result)
            {
                //string pythonExe = @"C:\Users\viga\Documents\GitHub\GFElevInterview\.venv\Scripts\python.exe";
                string pythonExe = System.IO.Path.Combine(System.IO.Path.GetFullPath(@".venv\Scripts\python.exe"));
                string pythonScript = @"C:\Users\viga\Documents\GitHub\GFElevInterview\Data\GFElevInterviewExcel.py";
                //string pythonScript = @"Data\GFElevInterviewExcel.py";

                //Relativ path til python.exe
                //ProcessStartInfo objekt
                Process process = new Process();
                process.StartInfo.FileName = pythonExe;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                #region Victor
                process.Start();
                process.StandardInput.WriteLine(@".venv\Scripts\activate.bat");
                process.StandardInput.WriteLine(@"cd Data\GFElevInterviewExcel.py");
                //process.StandardInput.WriteLine("python GFElevInterviewExcel.py");
                //process.StandardInput.Flush();
                process.StandardInput.Close();
                Console.WriteLine(process.StandardOutput.ReadToEnd());
                Console.Read();
                #endregion
                //ProcessStartInfo info = new ProcessStartInfo();
                //info.UseShellExecute = false;
                //info.RedirectStandardOutput = true;

                //info.FileName = pythonExe;
                //info.Arguments = string.Format("{0} {1}", pythonScript, openFile.FileName);

                //using (Process process = Process.Start(info))
                //{
                //    using (StreamReader reader = process.StandardOutput)
                //    {
                //        string _result = reader.ReadToEnd();
                //        Console.WriteLine();
                //    }
                //}

                //Process.Start(info);
            }
            Console.WriteLine(openFile.FileName);
            Console.WriteLine();
        }

        #region DatabaseQueries
        
        #endregion
        #region Events
        #region Knap metoder
        private void SPS_Click(object sender, RoutedEventArgs e)
        {
            OpdaterDataGrid(new DbTools().VisSPS());
        }

        private void EUD_Click(object sender, RoutedEventArgs e)
        {
            OpdaterDataGrid(new DbTools().VisEUD());
        }

        private void RKV_Click(object sender, RoutedEventArgs e)
        {
            OpdaterDataGrid(new DbTools().VisRKV());
        }

        private void Merit_Click(object sender, RoutedEventArgs e)
        {
            OpdaterDataGrid(new DbTools().VisMerit());
        }

        private void visAlle_Click(object sender, RoutedEventArgs e)
        {
            SkoleDropDown.SelectedIndex = -1;
            OpdaterDataGrid(new DbTools().VisAlle());
        }
        #endregion

        private void Open_Merit_Click(object sender, RoutedEventArgs e)
        {
            //TODO Reduce redundancy
            //if (elev == null)
            //{
            //    return;
            //}
            ÅbenFilPlacering(elev.MeritFilNavn);
        }

        private void Open_RKV_Click(object sender, RoutedEventArgs e)
        {
            //TODO Reduce redundancy
            //if (elev == null)
            //{
            //    return;
            //}
            ÅbenFilPlacering(elev.RKVFilNavn);
        }

        private void ExportMerit_Click(object sender, RoutedEventArgs e)
        {
            if (AlertBoxes.OnExport())
            {
                AdminTools.KombinerMeritFiler();
            }
        }

        private void ExportRKV_Click(object sender, RoutedEventArgs e)
        {
            if (AlertBoxes.OnExport())
            {
                AdminTools.ZipRKVFiler();
            }
        }

        private void SkoleDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == -1)
            {
                return;
            }
            string skole = (sender as ComboBox).SelectedItem.ToString();

            if (skole.Contains(' '))
            {
                string ændretSkole = skole.Substring(0, skole.IndexOf(' '));
                Console.WriteLine();
                //Merit forløb
                if (skole.Contains('+'))
                {
                    //Tekst fra skole variablen, fra start til mellemrummet.
                    //Også skal vi sætte det rigtige fagniveau.
                    OpdaterDataGrid(new DbTools().VisSkole(ændretSkole, FagNiveau.F, true));
                }
                //Ingen merit
                else
                {
                    OpdaterDataGrid(new DbTools().VisSkole(ændretSkole, FagNiveau.E, false));
                }
            }
            else
            {
                OpdaterDataGrid(new DbTools().VisSkole(skole));
            }

        }
        //TODO kan sætte elev som tom række
        private void elevTabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            elev = (sender as DataGrid).SelectedItem as ElevModel;

            if (elev == null)
            {
                Open_Merit.IsEnabled = false;
                Open_RKV.IsEnabled = false;
                return;
            }

            if (elev.danskNiveau == FagNiveau.Null)
                Open_Merit.IsEnabled = false;
            else
                Open_Merit.IsEnabled = true;


            if (elev.elevType == ElevType.Null)
                Open_RKV.IsEnabled = false;
            else
                Open_RKV.IsEnabled = true;

            //Er eleven færdig med interview?
            //Hvis ja, enable knap,
            //Hvis nej, disable knap.

        }
        #endregion

        private void TilføjKnp_Click(object sender, RoutedEventArgs e)
        {
            ÅbenFil();
        }
    }
}
