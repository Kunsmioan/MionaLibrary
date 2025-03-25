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

        public static bool validAuthorName(params string[] names)
        {
            // Kiểm tra từng tên trong danh sách
            foreach (var name in names)
            {
                // Nếu tên rỗng hoặc null, trả về false
                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }

                // Kiểm tra từng ký tự trong tên
                foreach (char c in name)
                {
                    // Cho phép: chữ cái, khoảng trắng, dấu chấm, dấu gạch nối
                    if (!char.IsLetter(c) && !char.IsWhiteSpace(c) && c != '.' && c != '-')
                    {
                        return false; // Nếu gặp ký tự không hợp lệ, trả về false
                    }
                }
            }

            // Nếu tất cả tên đều hợp lệ, trả về true
            return true;
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

        public static string legitAuthoName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            // Split the name by spaces while preserving special characters like '.' and '-'
            var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Process each word
            var processedWords = words.Select(word =>
            {
                // Handle special cases like "J.K." or "Dr."
                if (word.Contains('.') || word.Contains('-'))
                {
                    return string.Join("", word.Split('.', '-')
                        .Where(part => !string.IsNullOrEmpty(part)) // Remove empty parts
                        .Select(part => char.ToUpper(part[0]) + part.Substring(1).ToLower()));
                }

                // Capitalize the first letter and lowercase the rest
                return char.ToUpper(word[0]) + word.Substring(1).ToLower();
            });

            // Join the processed words with a single space
            return string.Join(" ", processedWords);
        }

        // Kiểm tra xem chuỗi có phải là số không
        public static bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }

        // positive number
        public static bool IsPositiveInteger(string input)
        {
            if (int.TryParse(input, out int result))
            {
                return result > 0;
            }
            return false;
        }


    }
}
