using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GFElevInterview.Data;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for RkvView.xaml
    /// </summary>
    public partial class RkvView : UserControl
    {
        public RkvView()
        {
            InitializeComponent();
        }

        private void Udskriv_Click(object sender, RoutedEventArgs e)
        {
            BlanketUdskrivning blanket = new BlanketUdskrivning();
            blanket.UdskrivningRKV();
        }
    }
}
