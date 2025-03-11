using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MionaLibrary
{
    internal class InputValidator
    {
        public static bool TextBoxesIsNotEmpty(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool TextBoxIsNotEmpty(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                return false;
            }
            return true;
        }
        public static bool PWIsNotEmpty(params PasswordBox[] pwBoxes)
        {
            foreach (var pwBox in pwBoxes)
            {
                if (string.IsNullOrWhiteSpace(pwBox.Password))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool DateIsNotEmpty(params ComboBox[] dateComboBoxes)
        {
            foreach (var comboBox in dateComboBoxes)
            {
                if (comboBox.SelectedItem == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
