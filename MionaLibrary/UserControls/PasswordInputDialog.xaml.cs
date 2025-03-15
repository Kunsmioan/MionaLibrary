using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MionaLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for PasswordInputDialog.xaml
    /// </summary>
    public partial class PasswordInputDialog : Window
    {
        public string Password { get; private set; }
        public PasswordInputDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Password = pbPassword.Password; // Store the entered password
            DialogResult = true; // Close the dialog with "OK" result
            Close();
        }

        // Handle Cancel button click
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Close the dialog with "Cancel" result
            Close();
        }
    }
}
