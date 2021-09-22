using System.Windows.Controls;

namespace GFElevInterview.Data
{
    public static class UdfyldBlanket
    {
        public static void UdfyldComboBox(ComboBox combobox, int index) {
            combobox.SelectedIndex = index;
        }

        public static void UdfyldRadioButton(RadioButton rbJa, RadioButton rbNej, bool? erJaValgt) {
            RadioButton rb = (bool)erJaValgt ? rbJa : rbNej;
            rb.IsChecked = true;
        }

        public static void UdfyldComboBox(ComboBox combobox, string navn) {
            combobox.SelectedItem = navn;
        }
    }
}