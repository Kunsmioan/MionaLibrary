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
                txtFirstName.Text = reader.FirstName;
                txtLastName.Text = reader.LastName;
                txtUsername.Text = reader.Username;
                dpDOB.SelectedDate = reader.Birthday;
                txtPhone.Text = reader.Phone;
            }
            else
            {
                MessageBox.Show("User data is missing!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.Key == Key.Enter)
            {
                //
            }
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
            (!InputValidator.validName(txtFirstName.Text, txtLastName.Text), "Name contains invalid characters!"),
            (!InputValidator.textBoxsLength(txtFirstName, txtLastName, txtUsername), "Text fields must be 5-50 characters!"),
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

            // Create the User object

            reader.Username = (txtUsername.Text);
            reader.FirstName = InputValidator.legitName(txtFirstName.Text);
            reader.LastName = InputValidator.legitName(txtLastName.Text);
            reader.Birthday = dpDOB.SelectedDate.Value;
            reader.Password = reader.Password;
            reader.Gender = GetSelectedGender();
            reader.Role = "User";
            reader.Phone = txtPhone.Text;

            try
            {
                // Ensure _userServices is initialized before use
                _userServices = new UserServices();

                // Call the register service to register the user
                _userServices.Update(reader);

                MessageBox.Show("Update successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                //ReturnToLogin(sender, e);
            }
            catch (InvalidOperationException ex)
            {
                // Log and display detailed error
                MessageBox.Show($"Update failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is ReaderWindow rw)
            {
                User? reader = rw.GetReader();
                //if (reader == null)
                //{
                //    MessageBox.Show("No user is selected. Please select a user first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                var profileControl = new ProfileControl();
                profileControl.SetUser(reader);


                // Thay thế nội dung hiện tại bằng BookDetailsControl
                rw.MainContent.Content = profileControl;
            }
            else
            {
                MessageBox.Show("Không tìm thấy cửa sổ ReaderWindow.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
