using GFElevInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GFElevInterview.Data;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace GFElevInterview.Views
{
    /// <summary>
    /// Interaction logic for WordView.xaml
    /// </summary>
    public partial class VisitationsView : UserControl, IBlanket
    {
        public VisitationsView()
        {
            InitializeComponent();
            

        }

        public bool Frem(out IBlanket nextBlanket) {

            IEnumerable<RadioButton> spsRadioButton = spsSupportGroup.Children.OfType<RadioButton>();
            IEnumerable<RadioButton> eudRadioButton = eudSupportGroup.Children.OfType<RadioButton>();

            if(educationComboBox.SelectedIndex >=0)
            {
                educationArea.BorderBrush = Brushes.Gray;
            }
            else
            {
                educationArea.BorderBrush = Brushes.Red;
            }
            if(educationAdresseComboBox.SelectedIndex >=0)
            {
                educationAdresse.BorderBrush = Brushes.Gray;
            }
            else
            {
                educationAdresse.BorderBrush = Brushes.Red;
            }
            if ((bool)spsSupportJa.IsChecked || (bool)spsSupportNej.IsChecked)
            {
                spsSupport.BorderBrush = Brushes.Gray;
            }
            else
            {
                spsSupport.BorderBrush = Brushes.Red;
            }
            if ((bool)eudSupportJa.IsChecked || (bool)eudSupportNej.IsChecked)
            {
                eudSupport.BorderBrush = Brushes.Gray;
            }
            else
            {
                eudSupport.BorderBrush = Brushes.Red;
            }
            if(educationComboBox.SelectedIndex >= 0 && educationAdresseComboBox.SelectedIndex >= 0 )
            {


                nextBlanket = new VisitationsView(); // skal skiftes til RkvView()
                return true;
            }
            nextBlanket = this;
            return false;



        }

        public bool Tilbage(out IBlanket previousBlanket) {
            throw new NotImplementedException();
        }
    }
}
