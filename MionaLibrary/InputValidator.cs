using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MionaLibrary
{
    internal class InputValidator
    { 
        //check valid by LINQ
        public static bool validName(params string[] names)
        {
            return !names.Any(name => name.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)));
        }

        //check length of textboxes
        public static bool textBoxsLength(params TextBox[] textBoxes)
        {
            return textBoxes.All(tb => tb.Text.Length is >= 2 and <= 49);
        }

        //check length of passwordboxes
        public static bool pwBoxsLength(params PasswordBox[] pwBoxes)
        {
            return pwBoxes.All(pb => pb.Password.Length is >= 4 and <= 254);
        }

        //check if textboxes are empty
        public static bool TextBoxesIsNotEmpty(params TextBox[] textBoxes)
        {
            return textBoxes.All(tb => !string.IsNullOrWhiteSpace(tb.Text));
        }

        //check textboxe empty
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
            return pwBoxes.All(pb => !string.IsNullOrWhiteSpace(pb.Password));
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

        //check phone valid
        public static bool validPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Regex: Allows optional '+' at start, digits, spaces, dashes, and parentheses
            string pattern = @"^\+?[0-9\s\-()]{7,15}$";

            return Regex.IsMatch(phone, pattern);
        }

        //return format string
        public static string legitName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            return string.Join(" ", name
                .Split(' ', StringSplitOptions.RemoveEmptyEntries) // Handle multiple spaces
                .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower())); // Capitalize each word
        }
    }
}
