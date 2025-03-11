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
            MainWindow mv = new MainWindow();
            mv.Show();
            this.Close();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
                if (!checkNotEmpty()) return;

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
                    Username = txtUsername.Text,
                    FullName = txtFirstName.Text + " " + txtLastName.Text,
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
        private bool checkNotEmpty()
        {
            if (!InputValidator.TextBoxIsNotEmpty(txtFirstName))
            {
                MessageBox.Show("Please enter first name!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!InputValidator.TextBoxIsNotEmpty(txtLastName))
            {
                MessageBox.Show("Please enter last name!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!InputValidator.TextBoxIsNotEmpty(txtUsername))
            {
                MessageBox.Show("Please enter username!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!InputValidator.TextBoxesIsNotEmpty(txtRePasswordVisible, txtPasswordVisible))
            {
                MessageBox.Show("Please enter password!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!InputValidator.PWIsNotEmpty(pbPassword, pbConfirmPassword))
            {
                MessageBox.Show("Password must not be empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
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
