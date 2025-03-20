using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.IdentityModel.Tokens;
using MionaLibrary_DAL.Entity;
using MionaLibrary_Services.Services;

namespace MionaLibrary
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        LoginServices? _loginServices;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(pbPassword.Password))
            {
                MessageBox.Show("Username or password must not be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _loginServices = new();
                // Authenticate the user
                User? userCheck = _loginServices.Authen(txtUsername.Text, pbPassword.Password);

                // Check if the user is found and valid
                if (userCheck == null)
                {
                    MessageBox.Show("Username or password is wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    
                    // Proceed only if user is authenticated successfully
                    string role = userCheck.Role.ToString();

                    // Redirect based on role
                    if (role == "User")
                    {
                        ReaderWindow readerWindow = new ReaderWindow();
                        readerWindow.SetReader(userCheck);
                        readerWindow.Show();
                        this.Close();
                    }
                    else if (role == "Manager")
                    {
                        ManagerWindow managerWindow = new ManagerWindow();
                        managerWindow.SetManager(userCheck);
                        managerWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid role!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e); // Trigger the login button click event
            }
        }



        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e); // Trigger the login button click event
            }
        }
        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            // Toggle the visibility of the password between PasswordBox and TextBox
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

            // Toggle the visibility of the icons
            EyeSlashIcon.Visibility = EyeSlashIcon.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            EyeIcon.Visibility = EyeIcon.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPasswordVisible.Text = pbPassword.Password;
        }
        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
             //Open Register Page
            Register register = new Register();  // Ensure Register.xaml exists
            register.Show();

            //Close the Login Page(Optional)
            this.Close();
        }


    }
}
