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
    /// Interaction logic for WordView.xaml
    /// </summary>
    public partial class WordView : UserControl, IBlanket
    {
        public WordView()
        {
            InitializeComponent();

        }

        public bool Frem(out IBlanket nextBlanket) {
            throw new NotImplementedException();
        }

        public bool Tilbage(out IBlanket previousBlanket) {
            throw new NotImplementedException();
        }
    }
}
