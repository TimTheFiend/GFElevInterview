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
using Microsoft.Data.Sqlite;
using System.Linq;
using config = System.Configuration.ConfigurationManager;


namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for LederSide.xaml
    /// </summary>
    public partial class LederSide : UserControl
    {
        private DbTools db;

        public LederSide()
        {
            InitializeComponent();
            InitialiseView();
            InitializeComboBox();
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
                                      where e.uddannelsesLængdeIUger == 16
                                      select e).ToList();
            OpdaterDataGrid(elever);
        }

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
    }
}
