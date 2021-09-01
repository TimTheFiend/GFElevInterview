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


namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for LederSide.xaml
    /// </summary>
    public partial class LederSide : UserControl
    {
        private DbTools db;
        private ElevModel elev;
        private string blanketMappe;

        public LederSide()
        {
            InitializeComponent();
            InitialiseView();
            InitializeComboBox();

            blanketMappe = config.AppSettings.Get("outputMappe");
        }

        //On Constructor call
        private void InitialiseView()
        {
            db = new DbTools();
            InitializeComboBox();
            InitialiserDataGrid();
        }

        private void InitialiserDataGrid()
        {
            VisAlle();
        }

        //Putter info ind fra App.Config i ComboBox
        private void InitializeComboBox()
        {
            List<string> uddannelsesAdresser = new List<string>() {
                config.AppSettings.Get("ballerup"),
                config.AppSettings.Get("lyngby"),
                config.AppSettings.Get("frederiksberg")
            };
            SkoleDropDown.ItemsSource = uddannelsesAdresser;
        }

        private void OpdaterDataGrid(List<ElevModel> elevData)
        {
            elevTabel.ItemsSource = elevData;
        }

        #region DatabaseQueries
        private void VisAlle()
        {
            List<ElevModel> elever = (from e in db.Elever
                                      select e).ToList();
            OpdaterDataGrid(elever);
        }

        private void VisSkole(string skole)
        {
            List<ElevModel> elever = (from e in db.Elever
                                      where e.uddannelseAdresse == skole
                                      select e).ToList();
            OpdaterDataGrid(elever);
        }

        private void VisSPS()
        {
            List<ElevModel> elever = (from e in db.Elever
                                      where e.sps == true
                                      select e).ToList();
            OpdaterDataGrid(elever);
        }

        private void VisEUD()
        {
            List<ElevModel> elever = (from e in db.Elever
                                      where e.eud == true
                                      select e).ToList();
            OpdaterDataGrid(elever);
        }

        private void VisRKV()
        {
            List<ElevModel> elever = (from e in db.Elever
                                      where e.elevType != 0
                                      select e).ToList();
            OpdaterDataGrid(elever);
        }
        private void VisMerit()
        {
            List<ElevModel> elever = (from e in db.Elever
                                      where e.danskNiveau > 0
                                      select e).ToList();
            OpdaterDataGrid(elever);
        } 
        #endregion

        #region Knap metoder
        private void SPS_Click(object sender, RoutedEventArgs e)
        {
            VisSPS();
        }

        private void EUD_Click(object sender, RoutedEventArgs e)
        {
            VisEUD();
        }

        private void RKV_Click(object sender, RoutedEventArgs e)
        {
            VisRKV();
        }

        private void Merit_Click(object sender, RoutedEventArgs e)
        {
            VisMerit();
        }

        private void visAlle_Click(object sender, RoutedEventArgs e)
        {
            VisAlle();
        } 
        #endregion

        private void SkoleDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string skole = (sender as ComboBox).SelectedItem.ToString();

            VisSkole(skole);
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

        private void Open_Merit_Click(object sender, RoutedEventArgs e)
        {
            //TODO Reduce redundancy
            //if (elev == null)
            //{
            //    return;
            //}
            OpenExploreOnFile(elev.MeritFilNavn);
        }

        private void Open_RKV_Click(object sender, RoutedEventArgs e)
        {
            //TODO Reduce redundancy
            //if (elev == null)
            //{
            //    return;
            //}
            OpenExploreOnFile(elev.RKVFilNavn);
        }

        private void OpenExploreOnFile(string blanketNavn)
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
    }
}
