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
using Microsoft.Data.SqlClient;
using MionaLibrary_DAL.Entity;
using MionaLibrary_DAL.Repository;
using MionaLibrary_Services.Services;

namespace MionaLibrary
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        UserServices? _userServices;
        public Register()
        {
            InitializeComponent();
        }
        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPasswordVisible.Text = pbPassword.Password; // Lưu giá trị mật khẩu mới
        }

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (pbPassword.Visibility == Visibility.Visible)
            {
                pbPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
                txtPasswordVisible.Text = pbPassword.Password;  // Copy password content
            }
            else
            {
                pbPassword.Visibility = Visibility.Visible;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                pbPassword.Password = txtPasswordVisible.Text;  // Copy content from visible TextBox back
            }

            EyeSlashIconP.Visibility = EyeSlashIconP.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            EyeIconP.Visibility = EyeIconP.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void pbConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtRePasswordVisible.Text = pbConfirmPassword.Password;
        }

        private void ToggleRePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (txtRePasswordVisible.Visibility == Visibility.Visible)
            {
                pbConfirmPassword.Visibility = Visibility.Visible;
                txtRePasswordVisible.Visibility = Visibility.Collapsed;
                pbConfirmPassword.Password = txtRePasswordVisible.Text;  // Đồng bộ lại mật khẩu
            }
            else
            {
                pbConfirmPassword.Visibility = Visibility.Collapsed;
                txtRePasswordVisible.Visibility = Visibility.Visible;
                txtRePasswordVisible.Text = pbConfirmPassword.Password;  // Đồng bộ lại mật khẩu
            }

            EyeSlashIconRP.Visibility = EyeSlashIconRP.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            EyeIconRP.Visibility = EyeIconRP.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.Key == Key.Enter)
            {
                Register_Click(sender, e);
            }
        }
        private void ReturnToLogin(object sender, RoutedEventArgs e)
        {
            Login mv = new();
            mv.Show();
            this.Close();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput()) return;

            // Ensure date is selected and valid
            if (dpDOB.SelectedDate == null || dpDOB.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Please select a valid date of birth (it cannot be in the future).", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if password and confirm password match
            if (!CheckPasswordsMatch()) return;

            // Create the User object
            User user = new User
            {
                Username = (txtUsername.Text),
                FirstName = InputValidator.legitName(txtFirstName.Text),
                LastName = InputValidator.legitName(txtLastName.Text),
                Birthday = dpDOB.SelectedDate.Value,
                Gender = GetSelectedGender(),
                Password = pbPassword.Password,
                Role = "User",
                Phone = txtPhone.Text,
            };

            // Try to register the user
            try
            {
                // Ensure _userServices is initialized before use
                _userServices = new UserServices();

                // Call the register service to register the user
                _userServices.Add(user);

                MessageBox.Show("Registration successful! Please log in to continue.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToLogin(sender, e);
            }
            catch (InvalidOperationException ex)
            {
                // Log and display detailed error
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Helper function to check if passwords match
        private bool CheckPasswordsMatch()
        {
            if (pbPassword.Password != pbConfirmPassword.Password || txtPasswordVisible.Text != txtRePasswordVisible.Text)
            {
                MessageBox.Show("Passwords do not match!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        // Helper function to check if all required fields are filled
        private bool checkInput()
        {
            try
            {
                var messages = new List<(bool condition, string message)>
        {
            (!InputValidator.TextBoxesIsNotEmpty(txtFirstName), "Please enter first name!"),
            (!InputValidator.TextBoxesIsNotEmpty(txtLastName), "Please enter last name!"),
            (!InputValidator.TextBoxesIsNotEmpty(txtUsername), "Please enter username!"),
            (!InputValidator.TextBoxesIsNotEmpty(txtRePasswordVisible, txtPasswordVisible), "Please enter password!"),
            (!InputValidator.PWIsNotEmpty(pbPassword, pbConfirmPassword), "Password must not be empty!"),
            (!InputValidator.validName(txtFirstName.Text, txtLastName.Text), "Name contains invalid characters!"),
            (!InputValidator.textBoxsLength(txtFirstName, txtLastName, txtUsername), "Text fields must be 5-50 characters!"),
            (!InputValidator.pwBoxsLength(pbPassword, pbConfirmPassword), "Password must be 5-255 characters!")
        };

                foreach (var (condition, message) in messages)
                {
                    if (condition)
                    {
                        MessageBox.Show(message, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"An error occurred during input validation:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }



        // Helper function to get the selected gender
        private string GetSelectedGender()
        {
            if (rbMale.IsChecked == true)
            {
                return "Male";
            }
            else if (rbFemale.IsChecked == true)
            {
                return "Female";
            }
            else
            {
                return "Custom";
            }
        }

        // Helper function to get the password

    }
}
