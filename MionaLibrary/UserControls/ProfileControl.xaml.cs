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
    /// Interaction logic for ProfileControl.xaml
    /// </summary>
    public partial class ProfileControl : UserControl
    {
        User? reader;
        UserServices _userServices;
        public ProfileControl()
        {
            InitializeComponent();
        }

        private void LoadProfileData()
        {
            if (reader != null)
            {
                // Ensure that reader is not null before setting the content
                tbUsername.Text = reader.Username.ToString();
                tbfullName.Text = reader.FullName.ToString();
                if (reader.Birthday.HasValue)
                {
                    tbBirthday.Text = reader.Birthday.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    tbBirthday.Text = string.Empty; // Or any other default value if birthday is null
                }
                tbGender.Text = reader.Gender.ToString();
                tbPhone.Text = reader.Phone.ToString();
            }
            else
            {
                //MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SetUser(User user)
        {
            reader = user;
            LoadProfileData();
            // You can refresh the UI or bind data here
        }

        private void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            // Show a confirmation dialog
            var confirmResult = MessageBox.Show("Do you want to delete your account?", "Alert",
                                                MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (confirmResult == MessageBoxResult.OK)
            {
                // Prompt the user to enter their password
                var passwordDialog = new PasswordInputDialog(); // Custom dialog for password input
                if (passwordDialog.ShowDialog() == true) // Show the dialog and check if "OK" was clicked
                {
                    string enteredPassword = passwordDialog.Password; // Get the entered password

                    // Verify the entered password (replace "storedPassword" with the actual stored password)
                    string storedPassword = reader.Password; // Example: Replace with your secure password retrieval logic
                    if (enteredPassword == storedPassword)
                    {
                        // Password matches, proceed with account deletion
                        _userServices = new();
                        _userServices.Remove(reader);
                        MessageBox.Show("Account deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Close the current window (parent window of this UserControl)

                        // Open the Login Window
                        Login loginWindow = new();
                        loginWindow.Show();
                        Window parentWindow = Window.GetWindow(this); // Get the parent window of the UserControl
                        parentWindow.Close();
                    }
                    else
                    {
                        // Password does not match
                        MessageBox.Show("Incorrect password. Account deletion canceled.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // User canceled the password input dialog
                    MessageBox.Show("Account deletion canceled.", "Canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
