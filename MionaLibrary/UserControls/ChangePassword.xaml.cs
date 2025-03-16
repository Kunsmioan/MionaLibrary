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
using MionaLibrary_DAL.Entity;
using MionaLibrary_Services.Services;

namespace MionaLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : UserControl
    {
        User? reader;
        UserServices userServices;
        public ChangePassword()
        {
            InitializeComponent();
        }

        public void SetReader(User user)
        {
            reader = user;
            // You can refresh the UI or bind data here
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

        private bool CheckPasswordsMatch()
        {
            if (pbNewPassword.Password != pbConfirmPassword.Password || txtNewPasswordVisible.Text != txtRePasswordVisible.Text)
            {
                MessageBox.Show("Passwords do not match!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }


        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.Key == Key.Enter)
            {
                //
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckPasswordsMatch()) return;

            if(txtNewPasswordVisible.Text == txtOldPasswordVisible.Text)
            {
                MessageBox.Show("New password must be different from the old one!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (reader != null)
            {
                // Check if old password is correct
                if (reader.Password != txtOldPasswordVisible.Text)
                {
                    MessageBox.Show("Old password is incorrect!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // Update password
                reader.Password = txtNewPasswordVisible.Text;
                // Update the user in the database
                userServices = new();
                userServices.Update(reader);
                MessageBox.Show("Password changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
