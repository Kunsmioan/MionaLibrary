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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using MionaLibrary_DAL.Entity;
using MionaLibrary_Services.Services;

namespace MionaLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for UpdateProfileControl.xaml
    /// </summary>
    public partial class UpdateProfileControl : UserControl
    {
        User? reader;
        UserServices? _userServices;
        public UpdateProfileControl()
        {
            InitializeComponent();
        }
        public void SetReader(User user)
        {
            reader = user;
            loadData();
        }

        public void loadData()
        {
            if (reader != null)
            {
                txtUsername.Text = reader.Username;
                txtFirstName.Text = reader.FullName;
                txtLastName.Text = reader.FullName;
                txtUsername.Text = reader.Username;
                dpDOB.SelectedDate = reader.Birthday;
                txtPhone.Text = reader.Phone;
            }
            else
            {
                MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void pbOldPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtOldPasswordVisible.Text = pbOldPassword.Password; // Lưu giá trị mật khẩu mới
        }

        private void ToggleOldPasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (pbOldPassword.Visibility == Visibility.Visible)
            {
                pbOldPassword.Visibility = Visibility.Collapsed;
                txtOldPasswordVisible.Visibility = Visibility.Visible;
                txtOldPasswordVisible.Text = pbOldPassword.Password;  // Copy password content
            }
            else
            {
                pbOldPassword.Visibility = Visibility.Visible;
                txtOldPasswordVisible.Visibility = Visibility.Collapsed;
                pbOldPassword.Password = txtOldPasswordVisible.Text;  // Copy content from visible TextBox back
            }

            EyeSlashIconP.Visibility = EyeSlashIconP.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            EyeIconP.Visibility = EyeIconP.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void pbNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtNewPasswordVisible.Text = pbNewPassword.Password; // Lưu giá trị mật khẩu mới
        }

        private void ToggleNewPasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (txtNewPasswordVisible.Visibility == Visibility.Visible)
            {
                pbNewPassword.Visibility = Visibility.Visible;
                txtNewPasswordVisible.Visibility = Visibility.Collapsed;
                pbNewPassword.Password = txtNewPasswordVisible.Text;  // Đồng bộ lại mật khẩu
            }
            else
            {
                pbNewPassword.Visibility = Visibility.Collapsed;
                txtNewPasswordVisible.Visibility = Visibility.Visible;
                txtNewPasswordVisible.Text = pbNewPassword.Password;  // Đồng bộ lại mật khẩu
            }

            EyeSlashIconNew.Visibility = EyeSlashIconNew.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            EyeIconNew.Visibility = EyeIconNew.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
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
                //
            }
        }

        private bool CheckPasswordsMatch()
        {
            if (pbNewPassword.Password != pbConfirmPassword.Password || txtNewPasswordVisible.Text != txtRePasswordVisible.Text)
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
            (!InputValidator.TextBoxesIsNotEmpty(txtRePasswordVisible, txtNewPasswordVisible), "Please enter password!"),
            (!InputValidator.PWIsNotEmpty(pbNewPassword, pbConfirmPassword), "Password must not be empty!"),
            (!InputValidator.validName(txtFirstName.Text, txtLastName.Text), "Name contains invalid characters!"),
            (!InputValidator.textBoxsLength(txtFirstName, txtLastName, txtUsername), "Text fields must be 5-50 characters!"),
            (!InputValidator.pwBoxsLength(pbNewPassword, pbConfirmPassword), "Password must be 5-255 characters!")
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

        private void Submit_Click(object sender, RoutedEventArgs e)
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
                FullName = InputValidator.legitName(txtFirstName.Text) + " " + InputValidator.legitName(txtLastName.Text),
                Birthday = dpDOB.SelectedDate.Value,
                Gender = GetSelectedGender(),
                Password = pbNewPassword.Password,
                Role = "User",
                Phone = txtPhone.Text,
            };

            // Try to register the user
            try
            {
                // Ensure _userServices is initialized before use
                _userServices = new UserServices();

                // Call the register service to register the user
                _userServices.Update(user);

                MessageBox.Show("Update successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                //ReturnToLogin(sender, e);
            }
            catch (InvalidOperationException ex)
            {
                // Log and display detailed error
                MessageBox.Show($"Update failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
