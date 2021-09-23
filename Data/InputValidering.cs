using System.Windows.Controls;
using System.Windows.Media;

namespace GFElevInterview.Data
{
    public static class InputValidering
    {
        /// <summary>
        /// Validerer ét fag fra <see cref="Views.MeritBlanketView"/>, og ændre border hvis der er fejl.
        /// </summary>
        /// <param name="eJa">Eksamen Ja</param>
        /// <param name="eNej">Eksamen Nej</param>
        /// <param name="uJa">Undervisning Ja</param>
        /// <param name="uNej">Undervisning Nej</param>
        /// <param name="niveau">FagNiveau</param>
        /// <param name="border">`Border`en omkring elementet</param>
        /// <returns><c>true</c> hvis alt er udfyldt; ellers <c>false</c>.</returns>
        public static bool ValiderMerit(RadioButton eJa, RadioButton eNej, RadioButton uJa, RadioButton uNej, ComboBox niveau, Border border = null) {
            bool erValideret = ValiderToRadioButtons(eJa, eNej) && ValiderToRadioButtons(uJa, uNej) && ValiderComboBox(niveau);

            if (border != null) {
                HighlightBorderInputStatus(border, erValideret);
            }
            return erValideret;
        }

        /// <summary>
        /// Validere om <see cref="RadioButton"/> er udfyldt.
        /// </summary>
        /// <param name="rbtnJa">"Ja" Radiobutton</param>
        /// <param name="rbtNej">"Nej" Radiobutton</param>
        /// <param name="border">Borderen omkring elementet</param>
        /// <returns><c>true</c> hvis mindst én er tjekket; ellers <c>false</c>.</returns>
        public static bool ValiderToRadioButtons(RadioButton rbtnJa, RadioButton rbtNej, Border border = null) {
            //Gør `RadioButton` til reel bool (ikke Nullable<bool>)
            bool erValideret = (bool)rbtnJa.IsChecked || (bool)rbtNej.IsChecked;

            if (border != null) {
                HighlightBorderInputStatus(border, erValideret);
            }

            return erValideret;
        }

        /// <summary>
        /// Validere om <see cref="RadioButton"/> er udfyldt, og returnere værdien fra <c>rbtnJa</c>/>.
        /// </summary>
        /// <param name="rbtnJa">"Ja" Radiobutton</param>
        /// <param name="rbtNej">"Nej" Radiobutton</param>
        /// <param name="erJaValgt">Værdien fra `rbtnJa`</param>
        /// <param name="border">Borderen omkring elementet</param>
        /// <returns><c>true</c> hvis mindst én er tjekket; ellers <c>false</c>.</returns>
        public static bool ValiderToRadioButtons(RadioButton rbtnJa, RadioButton rbtNej, out bool erJaValgt, Border border = null) {
            //Gør `RadioButton` til reel bool (ikke Nullable<bool>)
            bool rbJa = (bool)rbtnJa.IsChecked;
            bool rbNej = (bool)rbtNej.IsChecked;

            erJaValgt = rbJa;

            bool manglerInput = rbJa || rbNej;

            if (border != null) {
                HighlightBorderInputStatus(border, manglerInput);
            }

            return manglerInput;
        }

        /// <summary>
        /// Tjekker om <see cref="ComboBox"/> er blevet udfyldt.
        /// </summary>
        /// <param name="comboBox">ComboBox elementet</param>
        /// <param name="border">`Border` rundt om elementet</param>
        /// <returns></returns>
        public static bool ValiderComboBox(ComboBox comboBox, Border border = null) {
            bool erVærdiValgt = comboBox.SelectedIndex > -1;

            if (border != null) {
                HighlightBorderInputStatus(border, erVærdiValgt);
            }

            return erVærdiValgt;
        }

        /// <summary>
        /// Highlighter <see cref="Border"/> hvis der er fejl.
        /// </summary>
        /// <param name="border">Border på elementet</param>
        /// <param name="harFejl">valideret bool</param>
        private static void HighlightBorderInputStatus(Border border, bool harFejl) {
            SolidColorBrush farve = harFejl ? Brushes.Gray : Brushes.Red;

            border.BorderBrush = farve;
        }
    }
}