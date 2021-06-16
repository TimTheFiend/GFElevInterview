using GFElevInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for BlanketView.xaml
    /// </summary>
    public partial class BlanketView : UserControl
    {
        IBlanket currentView;
        public BlanketView()
        {
            InitializeComponent();

            if (currentView == null) {
                currentView = new GFElevInterview.Views.MeritBlanketView();
            }

            MeritContent.Content = currentView;

            IBlanket _;
            btnTilbage.IsEnabled = currentView.Tilbage(out _);

            //btnWordView.IsEnabled = false;
        }

        

        private void SearchStudentBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchStudentTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        
        private void btnMeritView_Click(object sender, RoutedEventArgs e)
        {


            if (MeritContent.IsEnabled)
            {
                MeritContent.Content = new GFElevInterview.Views.WordView();
                btnWordView.IsEnabled = true;

                if(WordContent.IsEnabled)
                {
                    int age = 30;
                    if (age < 25)
                    {
                        //gem pdf file
                    }
                    else //åben RKV 
                    {
                        WordContent.Content = new GFElevInterview.Views.RkvView();
                        btnWordView.IsEnabled = true;
                    }
                }

            }
                                   
        }

        private void Tilbage_Click(object sender, RoutedEventArgs e)
        {
            
                MeritContent.Content = new GFElevInterview.Views.maritBlanket();
                btnWordView.IsEnabled = false;
            
           
        }
    }
}
