using System.Windows.Controls;

namespace GFElevInterview.Data
{
    public static class UdfyldBlanket
    {
        /// <summary>
        /// Sætter <see cref="ComboBox"/>s index til bestemt værdi.
        /// </summary>
        /// <param name="combobox">ComboBox hvis værdi skal ændres</param>
        /// <param name="index">Værdien der skal vælges fra ComboBox</param>
        public static void UdfyldComboBox(ComboBox combobox, int index) {
            combobox.SelectedIndex = index;
        }

        /// <summary>
        /// "Checker" <see cref="RadioButton"/> baseret på bool.
        /// </summary>
        /// <param name="rbJa">RadioButton for "Ja"</param>
        /// <param name="rbNej">Radiobutton for "Nej"</param>
        /// <param name="erJaValgt">Om "Ja" er "Checked"</param>
        public static void UdfyldRadioButton(RadioButton rbJa, RadioButton rbNej, bool? erJaValgt) {
            RadioButton rb = (bool)erJaValgt ? rbJa : rbNej;
            rb.IsChecked = true;
        }

        /// <summary>
        /// Sætter <see cref="ComboBox"/>s index til bestemt værdi.
        /// </summary>
        /// <param name="combobox">ComboBox hvis værdi skal ændres</param>
        /// <param name="navn">Teksten der står i ComboBox.</param>
        public static void UdfyldComboBox(ComboBox combobox, string navn) {
            combobox.SelectedItem = navn;
        }
    }
}