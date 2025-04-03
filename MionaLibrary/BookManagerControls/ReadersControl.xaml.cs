using MionaLibrary_DAL.Entity;
using MionaLibrary_Services.Services;
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

namespace MionaLibrary.BookManagerControls
{
    /// <summary>
    /// Interaction logic for ReadersControl.xaml
    /// </summary>
    public partial class ReadersControl : UserControl
    {
        UserServices _userServices = new UserServices();

        public ReadersControl()
        {
            InitializeComponent();
            LoadData(); 
        }

        public void LoadData()
        {
            UserDataGrid.ItemsSource = _userServices.GetAll();
        }

        private void ReaderDetailsAndBooksOnloan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button == null || button.Tag == null)
            {
                MessageBox.Show("Unable to identify the user to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (int.TryParse(button.Tag.ToString(), out int userId))
            {
                var userToDelete = _userServices.GetById(userId);

                if (userToDelete == null)
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"Are you sure you want to delete the user: {userToDelete.Username}?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _userServices.Remove(userToDelete);
                    LoadData();
                    MessageBox.Show($"User {userToDelete.Username} has been deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Invalid user ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}